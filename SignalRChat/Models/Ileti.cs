namespace SignalRChat.Models;

public class Ileti : IEntity
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string? GonderenAD { get; set; }
    public int? GonderenId { get; set; }
    public string? Mesaj { get; set; }
}