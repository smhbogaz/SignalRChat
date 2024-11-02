namespace SignalRChat.Models;

public class Mesajlar : IEntity
{
    public int Id { get; set; }
    public int MesajAtanId { get; set; }
    [MaxLength(100)]
    public string? Mesaj { get; set; }
    [MaxLength(100)]
    public string? MesajTarihi { get; set; }
}