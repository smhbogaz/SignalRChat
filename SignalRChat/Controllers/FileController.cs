namespace SignalRChat.Controllers;

public class FileController : Controller
{
    private readonly Globals _globals = Globals.Instance;

    [Route("/file/list/"), HttpGet] public IActionResult Index() => View(_globals.ListRow<FileMedia>());
    [Route("/file/upload/"), HttpGet] public IActionResult Upload() => View();

    [Route("/file/download/{guid}/"), HttpPost]
    public IActionResult DownloadFile(string guid)
    {
        FileMedia fileMedia = _globals.findRow<FileMedia>(("GuidId", guid));

        if (fileMedia == null)
            return NotFound("dosya bulunamadı"); // Dosya bulunamazsa 404 döner


        var filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot\\file", fileMedia.FileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound("dosya konumu bulunamadı"); // Dosya belirtilen konumda yoksa 404 döner

        if (fileMedia.ExpirationDate < DateTime.UtcNow)
            return NotFound("Dosyanın kullanım süresi dolmuştur.");
        
        if (!fileMedia.IsActive)
            return NotFound("Dosya indirme işlemine kapatılmış.");


        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        fileMedia.FileDownloadCount++;
        _globals.Db.SaveChanges();
        return File(fileBytes, "application/octet-stream", fileMedia.FileName);
    }

    [Route("/file/download/{guid}"), HttpGet]
    public IActionResult Download(string guid)
    {
        FileMedia fileMedia = _globals.findRow<FileMedia>(("GuidId", guid));
        if (fileMedia != null)
        {
            fileMedia.AccessCount++;
            _globals.Db.SaveChanges();
        }        
        return View(fileMedia);
    }

    [Route("file/delete/{id}/{guid}/"), HttpGet]
    public IActionResult DeleteFile(string guid, int id)
    {
        var fileMedia = _globals.findRow<FileMedia>(("GuidId", guid));

        if (fileMedia == null)
            return NotFound("dosya bulunamadı"); // Dosya bulunamazsa 404 döner

        var filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot\\file", fileMedia.FileName);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
        _globals.DeleteRow(fileMedia);
        _globals.Db.SaveChanges();

        return RedirectToAction("Index");
    }

    [Route("/file/upload/"), HttpPost]
    public async Task<IActionResult> Upload(string fileName, IFormFile? file, string description, DateTime expirationDate, string active)
    {
        try
        {
            bool isActive = active == "true";

            if (file != null)
            {

                await _globals.UploadFile(file, file.FileName, "wwwroot\\file");

                FileMedia fileMedia = new FileMedia();
                
                fileMedia.GuidId = Guid.NewGuid().ToString();
                fileMedia.FileName = file.FileName;
                fileMedia.DisplayFileName = fileName;
                fileMedia.FileLocation = Path.Combine(Environment.CurrentDirectory, "wwwroot\\file", fileName);
                fileMedia.FileType = file.ContentType;
                fileMedia.FileSize = (int)file.Length;
                fileMedia.Description = description;
                fileMedia.ExpirationDate = expirationDate;
                fileMedia.IsActive = isActive;
                
                await _globals.AddRowAsync(fileMedia);
                await _globals.Db.SaveChangesAsync();

                return Json(new { success = true, message = "Dosya Başarıyla Yüklendi" });
            }

            return Json(new { success = false, message = "Dosya Yüklenemedi" });
        }
        catch (Exception e)
        {
            return Json(new { success = false, message = e.Message });
        }
    }
}
