namespace SignalRChat.Models;

public class Members : IEntity
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(50)]
    public string? Email { get; set; }
    [MaxLength(50)]
    public string? Password { get; set; }
    [MaxLength(50)]
    public string? MembersIP { get; set; }
    public bool isBanned { get; set; }
    [MaxLength(50)]
    public string? Yetki { get; set; }
    public string? Resim { get; set; }
    public bool? Online { get; set; }
}