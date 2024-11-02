namespace SignalRChat.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task Contact(string name, string surname, string email, string phone, string message)
    {
        Globals globals = Globals.Instance;

        string[] passwords = globals.passwords;
        foreach (var t in passwords)
        {
            if (await globals.SendMailAsync(
                    "smtp.outlook.com",
                    587,
                    "sekorlockdogrulama@outlook.com",
                    t,
                    ["18sbogaz24@gmail.com"],
                    $"Websiteden yeni bir mesaj: {name}",
                    $"Ad-Soyad: {name}-{surname}\nMail: {email}\nTelefon: {phone}\nMesaj: {message}",
                    true))break;
        }
    }


    /// <summary>
    ///  Whatsapp Automation desktop app. 
    /// </summary>
    /// <param name="a"> Ad </param>
    /// <param name="s"> Soyad </param>
    /// <param name="p"> Program Nerede Kullanılıyor </param>
    /// <param name="c"> Sorununuzu Açıklayın </param>
    /// <param name="i"> İletişim Adresi </param>
    /// <returns> Dönüş değerleri </returns>
    [Route("wac")]
    public async Task<IActionResult> WhatsappAutomationContact(string a, string s, string p, string c, string i)
    {
        Globals globals = Globals.Instance;

        string sendText = @$"
            Ad Soyad: {a} {s}
            Program nerde kullanılıyor: {p}
            Sorun: {c}
            İletişim Adresi: {i}
                ";



        string[] passwords = globals.passwords;
        foreach (var t in passwords)
        {
            if (await globals.SendMailAsync(
                    "smtp.outlook.com",
                    587,
                    "sekorlockdogrulama@outlook.com",
                    t,
                    ["18sbogaz24@gmail.com"],
                    $"Whapsapp Automation - Yeni Bir Destek Talebin Var!",
                    sendText,
                    true)) return Ok();
        }
        return BadRequest();
    }
}