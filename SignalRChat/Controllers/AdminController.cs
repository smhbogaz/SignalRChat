namespace SignalRChat.Controllers;

public class AdminController : Controller
{
    public static string? SessionAuth { get; private set; }

    private readonly Globals _globals = Globals.Instance;

    private IActionResult AdminCheck(string viewName)
    {
        if (HttpContext.Session.GetString(nameof(SessionSaveValue.MemberYetki)) != nameof(Auth.Admin))
        {
            return RedirectToAction("Index", "User");
        }
        return View(viewName: viewName);
    }
    //sayfalar
    [Route("mesaj-listesi")]
    public IActionResult MessageList()
    {
        var methodame = MethodBase.GetCurrentMethod().Name;
        ViewBag.CurrentPageAction = methodame;
        return AdminCheck(methodame);
    }
    [Route("üye-güncelle")]
    public IActionResult UpdateMember()
    {
        var methodame = MethodBase.GetCurrentMethod().Name;
        ViewBag.CurrentPageAction = methodame;
        return AdminCheck(methodame);
    }
    [Route("üye-listesi")]
    public IActionResult MemberList()
    {
        var methodame = MethodBase.GetCurrentMethod().Name;
        ViewBag.CurrentPageAction = methodame;
        return AdminCheck(methodame);
    }
    [Route("mesaj-sil")]
    public IActionResult DeleteMessage()
    {
        var methodame = MethodBase.GetCurrentMethod().Name;
        ViewBag.CurrentPageAction = methodame;
        return AdminCheck(methodame);
    }
    [Route("öneriler")]
    public IActionResult Suggestions()
    {
        var methodame = MethodBase.GetCurrentMethod().Name;
        ViewBag.CurrentPageAction = methodame;
        return AdminCheck(methodame);
    }
    [Route("üye-sil")]
    public IActionResult DeleteMember()
    {
        var methodame = MethodBase.GetCurrentMethod().Name;
        ViewBag.CurrentPageAction = methodame;
        return AdminCheck(methodame);
    }
    [Route("[action]")]
    public IActionResult Index()
    {
        var methodame = MethodBase.GetCurrentMethod().Name;
        ViewBag.CurrentPageAction = methodame;
        SessionAuth = HttpContext.Session.GetString(nameof(SessionSaveValue.MemberYetki));
        return AdminCheck(methodame);
    }
    [Route("resim-sil")]
    public IActionResult DeleteImage()
    {
        var methodame = MethodBase.GetCurrentMethod().Name;
        ViewBag.CurrentPageAction = methodame;
        return AdminCheck(methodame);
    }


    //form action 
    [HttpPost]
    public async Task<IActionResult> UpdateMember(int id, string? name, string? email, string? password, bool isBanned, Auth auth, bool online)
    {
        var updateMembers = _globals.findRow<Members>(("Id", id));
        updateMembers.Password = password ?? updateMembers.Password;
        updateMembers.Email = email ?? updateMembers.Email;
        updateMembers.Name = name ?? updateMembers.Name;
        updateMembers.Yetki = auth.ToString();
        updateMembers.isBanned = isBanned;
        updateMembers.Online = online;
        await _globals.Db.SaveChangesAsync();

        return Json(new { success = true, message = "Güncelleme işlemi gerçekleştirildi" });
    }
    [HttpPost]
    public async Task<IActionResult> DeleteMessageForUser(int selectedUser)
    {
        List<Mesajlar> m = [.. _globals.FindRows<Mesajlar>(("MesajAtanId", selectedUser))!];
        _globals.DeleteRows(m);
        await _globals.Db.SaveChangesAsync();

        return Json(new { success = true, message = "Tüm mesajlar silindi" });
    }
    [HttpPost]
    public async Task<IActionResult> DeleteImageForUser(int id)
    {
        var deleteMember = _globals.findRow<Members>(("Id", id));
        var deleteImage = deleteMember.Resim;
        if (deleteImage is not null)
        {
            _globals.DeleteFile(deleteImage);
        }
        deleteMember.Resim = null;
        await _globals.Db.SaveChangesAsync();

        return Json(new { success = true, message = "Resim silme işlemi gerçekleştirildi" });
    }
    [HttpPost]
    public async Task<IActionResult> DeleteMessageForSingle(int id)
    {
        _globals.DeleteRow<Mesajlar>(_globals.findRow<Mesajlar>(("Id", id)));

        await _globals.Db.SaveChangesAsync();

        return Json(new { success = true, message = "Mesaj işlemi gerçekleştirildi" });
    }
    [HttpPost]
    public async Task<IActionResult> DeleteSuggestions(int id)
    {
        _globals.DeleteRow<Ileti>(_globals.findRow<Ileti>(("Id", id)));
        await _globals.Db.SaveChangesAsync();

        return Json(new { success = true, message = "Öneri Silindi" });
    }
    [HttpPost]
    public async Task<IActionResult> DeleteMemberForSingle(int id)
    {
        var deleteMember = _globals.findRow<Members>(("Id", id));
        var deleteImage = deleteMember.Resim;
        if (deleteImage is not null)
        {            
            _globals.DeleteFile(deleteImage);
        }
        _globals.DeleteRow(deleteMember);
        _globals.DeleteRows<Mesajlar>(_globals.FindRows<Mesajlar>(("MesajAtanId", deleteMember.Id)));
        await _globals.Db.SaveChangesAsync();

        return Json(new { success = true, message = "Silme işlemi gerçekleştirildi" });
    }

}