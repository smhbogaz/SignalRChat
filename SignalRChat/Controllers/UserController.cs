namespace SignalRChat.Controllers;

public class UserController : Controller
{
    public static int Id { get; private set; }

    public static string? Resim { get; private set; }

    public static string? Ad { get; private set; }

    public static string? Email { get; private set; }

    public static string? ToplamMesaj { get; private set; }

    public static string? SessionYetki { get; private set; }

    private readonly Globals _globals = Globals.Instance;

    [Route("bilgileri-güncelle")]
    public IActionResult UpdateInformation()
    {
        try
        {
            ViewBag.CurrentPageAction = MethodBase.GetCurrentMethod().Name;
            Id = int.Parse(HttpContext.Session.GetString(nameof(SessionSaveValue.MemberId)));
            Ad = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberName));
            SessionYetki = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberYetki));
            return View();
        }
        catch (Exception)
        {
            return RedirectToAction("Index", "ChatApp");
        }
    }
    [Route("hesap-bilgileri")]
    public IActionResult AccountInfo()
    {
        try
        {

            ViewBag.CurrentPageAction = MethodBase.GetCurrentMethod().Name;
            Id = int.Parse(HttpContext.Session.GetString(nameof(SessionSaveValue.MemberId)));
            Ad = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberName));
            SessionYetki = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberYetki));
            Email = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberEmail));
            ToplamMesaj = _globals.RowCount<Mesajlar>(("MesajAtanId", Id)).ToString();
            return View();
        }
        catch (Exception)
        {
            return RedirectToAction("Index", "ChatApp");
        }

    }
    [Route("eski-mesajlar")]
    public IActionResult OldMessages()
    {
        try
        {
            ViewBag.CurrentPageAction = MethodBase.GetCurrentMethod().Name;
            Id = int.Parse(HttpContext.Session.GetString(nameof(SessionSaveValue.MemberId)));
            Ad = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberName));
            SessionYetki = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberYetki));
            return View();
        }
        catch (Exception)
        {
            return RedirectToAction("Index", "ChatApp");
        }
    }
    [Route("görüş-öneri")]
    public IActionResult OpinionSuggestion()
    {
        try
        {
            ViewBag.CurrentPageAction = MethodBase.GetCurrentMethod().Name;
            Id = int.Parse(HttpContext.Session.GetString(nameof(SessionSaveValue.MemberId)));//sonlandı ise burası hata verir
            Ad = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberName));
            SessionYetki = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberYetki));
            Resim = _globals.findRow<Members>(("Id", Id)).Resim;
            return View();
        }
        catch (Exception)
        {
            return RedirectToAction("Index", "ChatApp");
        }
    }
    [Route("çıkış-yap")]
    public async Task<IActionResult> Logout()
    {
        try
        {

            var members = await _globals.FindRowAsync<Members>(("Id", int.Parse(HttpContext.Session.GetString(nameof(SessionSaveValue.MemberId)) ?? "")));
            members.Online = false;
            await _globals.Db.SaveChangesAsync();
        }
        catch (Exception)
        {
            // ignored
        }

        HttpContext.Session.Clear();
        return RedirectToAction("Index", "ChatApp");
    }
    //Default Route
    [Route("chat")]
    public IActionResult Index()
    {
        try
        {
            ViewBag.CurrentPageAction = MethodBase.GetCurrentMethod().Name;
            Id = int.Parse(HttpContext.Session.GetString(nameof(SessionSaveValue.MemberId)));
            Ad = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberName));
            SessionYetki = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberYetki));
            Resim = _globals.findRow<Members>(("Id", Id)).Resim;
            return View();
        }
        catch (Exception)
        {
            return RedirectToAction("Index", "chatapp");
        }
    }



    [HttpPost]
    public async Task<IActionResult> UpdateInformationForUser(string? fullName, string oldPassword, string? newPassword, IFormFile? image)
    {
        try
        {
            var id = int.Parse(HttpContext.Session.GetString(nameof(SessionSaveValue.MemberId)));
            var findMember = await _globals.FindRowAsync<Members>(("Id", id));


            if (findMember == null)
            {
                return Json( new {success= false, message= "Üye bulunamadı."} );
            }

            if (findMember.Password != oldPassword)
                return Json( new {success= false, message= "Eski şifreniz yanlış."} );

            findMember.Name = fullName ?? findMember.Name;
            findMember.Password = newPassword ?? findMember.Password;

            if (image != null)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();
                if (extension is ".jpg" or ".png")
                {
                    _globals.DeleteFile(findMember.Resim);

                    var imageFileName = $"{HttpContext.Session.GetString(nameof(SessionSaveValue.MemberId))}-{HttpContext.Session.GetString(nameof(SessionSaveValue.MemberName))}{extension}".Replace(" ", "");
                    await _globals.UploadImageFile(image, imageFileName);
                    findMember.Resim = imageFileName;
                }
                else
                {
                    return Json( new {success= false, message= "Yalnızca .jpg ve .png dosyaları desteklenir."} );
                }
            }

            await _globals.Db.SaveChangesAsync();
            return Json( new {success= true, message= "İşlem başarılı."} );
        }
        catch (Exception ex)
        {
            return Json( new {success= false, message= $"Hata oluştu: {ex.Message}"}) ;
        }
    }

    [HttpPost]
    public async Task<JsonResult> SendSuggestion(string opinion)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(opinion))
            {
                return Json(new { success = false, message = "Görüş alanı boş olamaz." });
            }

            Ileti ileti = new()
            {
                GonderenAD = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberName)),
                GonderenId = Convert.ToInt32(HttpContext.Session.GetString(nameof(SessionSaveValue.MemberId))),
                Mesaj = opinion
            };
            await _globals.AddRowAsync(ileti);
            await _globals.Db.SaveChangesAsync();

            return Json(new { success = true, message = "Görüşünüz başarıyla gönderildi." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
        }
    }

}