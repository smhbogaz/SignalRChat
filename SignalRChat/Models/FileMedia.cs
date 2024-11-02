namespace SignalRChat.Models;

public class FileMedia :IEntity
{
    [Key]
    public int Id { get; set; }
    public string GuidId { get; set; }
    public string FileName { get; set; }
    public string DisplayFileName { get; set; }
    public string FileLocation { get; set; }
    public string FileType { get; set; }
    public int FileSize { get; set; }
    public int FileDownloadCount { get; set; }
    public DateTime UploadDate { get; set; }= DateTime.Now;
    public string Description { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public int AccessCount { get; set; }
}
