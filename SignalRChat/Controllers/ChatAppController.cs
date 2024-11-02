namespace SignalRChat.Controllers;

public class ChatAppController(IAccountService accountService) : Controller
{
    private readonly Globals _globals = Globals.Instance;
    private readonly Members _uye = new();

    //sayfalar
    [Route("reset-password")]
    public IActionResult Resetpassword()
    {
        return View();
    }
    public IActionResult Index()
    {
        return View();
    }

    //form
    [HttpPost]
    public async Task<IActionResult> ChangePassword(string sessionValid, string? newPassword)
    {

        if (string.IsNullOrEmpty(sessionValid))
        {
            return Json(new { success = false, message = "Gelen kutunuza gönderilen kodu girmelisiniz" });
        }
        if (string.IsNullOrEmpty(newPassword))
        {
            return Json(new { success = false, message = "Şifreniz boş olamaz" });
        }

        if (HttpContext.Session.GetString("ResetPassword") == sessionValid)
        {
            var members = _globals.findRow<Members>(("Email", HttpContext.Session.GetString("Mail")));
            members.Password = newPassword;
            HttpContext.Session.Clear();
            await _globals.Db.SaveChangesAsync();

            return Json(new { success = true, message = "Şifre sıfırlama işleminiz gerçekleştirildi!", redirectUrl = Url.Action("Index") });
        }

        return Json(new { success = false, message = "Yanlış kod girdiniz lütfen tekrar kontrol edin" });
    }
    [HttpPost]
    public async Task<JsonResult> Register(string fullName, string email, string sifre)
    {
        if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || !email.Contains('@') || !email.Contains('.') || string.IsNullOrEmpty(sifre))
        {
            return Json(new { success = false, message = "Lütfen tüm alanları doğru bir şekilde doldurun." });
        }

        if (await _globals.AnyRowAsync<Members>(("Email", email)))
        {
            return Json(new { success = false, message = "Bu email adresi zaten kullanılıyor." });
        }

        HttpContext.Session.SetString("LoginMailCode", Random.Shared.Next(111111, 999999).ToString());
        try
        {

            bool mailSent = await _globals.SendMailAsync(
                 "mail.kurumsaleposta.com",
                 587,
                 "register@semihbogaz.com.tr",
                 "HİDDEN",
                 [email],
                 $"Sayın {fullName} Doğrulama Kodunuz",
                 HttpContext.Session.GetString("LoginMailCode") ?? "",
                 false
                 );


            if (mailSent)
            {
                HttpContext.Session.SetString("LoginName", fullName);
                HttpContext.Session.SetString("LoginEmail", email);
                HttpContext.Session.SetString("LoginPassword", sifre);
                return Json(new { success = true, message = "Doğrulama kodu gönderildi. Lütfen emailinizi kontrol edin.", redirectUrl = Url.Action("Verify", "ChatApp") });
            }

            return Json(new { success = false, message = "Mail gönderilemedi, lütfen tekrar deneyin." });
        }
        catch (SmtpException)
        {
            return Json(new { success = false, message = "Mail gönderiminde bir hata oluştu." });
        }
    }

    [HttpPost]
    public async Task<JsonResult> Login(string loginEmail, string loginPassword)
    {
        if (string.IsNullOrEmpty(loginEmail) || string.IsNullOrEmpty(loginPassword))
        {
            return Json(new { success = false, message = "Email veya şifre boş olamaz!" });
        }

        var account = accountService.Login(loginEmail, loginPassword);
        if (account != null)
        {
            if (account.isBanned && account.Yetki != nameof(Auth.Admin))
            {
                return Json(new { success = false, message = "Hesabınız yasaklanmış!" });
            }


            var online = await _globals.FindRowAsync<Members>(("Password", loginPassword), ("Email", loginEmail));
            online.Online = true;
            await _globals.Db.SaveChangesAsync();



            HttpContext.Session.SetString(nameof(SessionSaveValue.MemberName), account.Name ?? "");
            HttpContext.Session.SetString(nameof(SessionSaveValue.MemberEmail), account.Email ?? "");
            HttpContext.Session.SetString(nameof(SessionSaveValue.MemberPassword), account.Password ?? "");
            HttpContext.Session.SetString(nameof(SessionSaveValue.MemberId), account.Id.ToString());
            HttpContext.Session.SetString(nameof(SessionSaveValue.MemberYetki), account.Yetki ?? "");

            if (account.Yetki == nameof(Auth.Admin))
                return Json(new { success = true, message = "Admin paneline yönlendiriliyorsunuz...", redirectUrl = Url.Action("Index", "Admin") });
            

            return Json(new { success = true, message = "Kullanıcı paneline yönlendiriliyorsunuz...", redirectUrl = Url.Action("Index", "User") });
        }

        return Json(new { success = false, message = "Geçersiz email veya şifre!" });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword_(string resetEmail)
    {
        if (string.IsNullOrEmpty(resetEmail))
            return Json(new { success = false, message = "Hesabınıza kayıtlı email adresinizi girmelisiniz" });
        

        var members = await _globals.FindRowAsync<Members>(("Email", resetEmail), ("isBanned", false));

        if (members == null)
            return Json(new { success = false, message = "Email kayıtlarımızla eşleşmiyor veya banlandınız" });
        
        try
        {
            HttpContext.Session.SetString("ResetPassword", Random.Shared.Next(0000, 9999).ToString());
            HttpContext.Session.SetString("Mail", members.Email ?? "");

            if (await _globals.SendMailAsync(
                "mail.kurumsaleposta.com",
                587,
                "reset@semihbogaz.com.tr",
                "HİDDEN",
                [members.Email ?? ""],
                $"Sayın {members.Name} Şifre Sıfırlama Kodunuz",
                HttpContext.Session.GetString("ResetPassword") ?? "",
                false
                ))
                return Json(new { success = true, message = "Şifre sıfırlama kodu gönderildi,gelen kutunuzu kontrol ediniz", redirectUrl = Url.Action("ChangePassword") });

            return Json(new { success = false, message = "Şifre sıfırlama kodu gönderilemedi,daha sonra tekrar deneyim" });

        }
        catch (Exception)
        {
            return Json(new { success = false, message = "İşlem sırasında hata oluştu" });
        }
    }

    public IActionResult Changepassword()
    {
        if (HttpContext.Session.GetString("ResetPassword") is not null)
        {
            return View();
        }
        return RedirectToAction("Index");

    }




    [HttpPost]
    public async Task<IActionResult> Verify(string? accountCode)
    {
        if (accountCode is not null)
        {
            if (accountCode == HttpContext.Session.GetString("LoginMailCode"))
            {

                _uye.Name = HttpContext.Session.GetString("LoginName");
                _uye.Email = HttpContext.Session.GetString("LoginEmail");
                _uye.Password = HttpContext.Session.GetString("LoginPassword");
                _uye.isBanned = false;
                _uye.Yetki = nameof(Auth.Personel);
                _uye.Online = true;
                await _globals.AddRowAsync(_uye);
                await _globals.Db.SaveChangesAsync();
                HttpContext.Session.Clear();
                HttpContext.Session.SetString(nameof(SessionSaveValue.MemberName), _uye.Name ?? "");
                HttpContext.Session.SetString(nameof(SessionSaveValue.MemberEmail), _uye.Email ?? "");
                HttpContext.Session.SetString(nameof(SessionSaveValue.MemberPassword), _uye.Password ?? "");
                HttpContext.Session.SetString(nameof(SessionSaveValue.MemberId), _uye.Id.ToString());
                HttpContext.Session.SetString(nameof(SessionSaveValue.MemberYetki), _uye.Yetki);
                return Json(new { success = true, message = "Doğrulama kodu başarılı, yönlendiriliyorsunuz", redirectUrl = Url.Action("Index", "User") });
            }
            return Json(new { success = false, message = "Yanlış kod tekrar deneyiniz." });
        }
        return Json(new { success = false, message = "Doğrulama kodunu girmelisiniz." });
    }



    public IActionResult Verify()
    {
        if (HttpContext.Session.GetString("LoginMailCode") is not null)
            return View();
        return RedirectToAction("Index", "ChatApp");
    }

}