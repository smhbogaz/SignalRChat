namespace SignalRChat.Models;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();                
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlCon"));
        }
    }
    public DbSet<Members> Members { get; set; }
    public DbSet<Mesajlar> Mesajlar { get; set; }
    public DbSet<Ileti> Ileti { get; set; }
    public DbSet<FileMedia> File { get; set; }
}