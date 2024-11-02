using System.Diagnostics.CodeAnalysis;

namespace SignalRChat.Other;

[SuppressMessage("ReSharper", "NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract")]
public class Globals : IDisposable
{

    private static readonly Globals instance;
    private static readonly object _lock = new();
    public static Globals Instance
    {
        get
        {
            lock (_lock)
            {
                return instance ?? new Globals();
            }
        }
    }
    private Globals() { }




    public AppDbContext? Db = new();
    public readonly string[] passwords = [
        "HİDDEN!",
        "HİDDEN!!",
        "HİDDEN!!!",
        "HİDDEN!!!!",
        "HİDDEN!!!!!",
        "HİDDEN!!!!!!",
        "HİDDEN!!!!!!!",
        "HİDDEN!!!!!!!!",
        "HİDDEN!!!!!!!!!",
        "HİDDEN!!!!!!!!!!",
    ];

    private readonly string UploadImageDirectory = "wwwroot\\img\\user";
    public async Task<bool> SendMailAsync(
        string host,
        int port,
        string userName,
        string password,
        string[] toMail,
        string subject,
        string body,
        bool enableSsl)
    {
        try
        {
            using SmtpClient istemci = new(host);
            istemci.Port = port;
            istemci.Credentials = new NetworkCredential(userName, password);
            istemci.EnableSsl = enableSsl;
            MailMessage msj = new()
            {
                From = new MailAddress(userName),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            foreach (string s in toMail)
            {
                msj.To.Add(s);
            }
            await istemci.SendMailAsync(msj);

            return true;
        }
        catch { return false; }

    }

    /*----------*/
    public async Task<T?> FindRowAsync<T>(params (string propertyName, object value)[] conditions) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression? finalExpression = null;
        foreach (var (propertyName, value) in conditions)
        {
            MemberExpression property = Expression.Property(parameter, propertyName);
            BinaryExpression condition = Expression.Equal(property, Expression.Constant(value));

            finalExpression = finalExpression == null ? condition : Expression.AndAlso(finalExpression, condition);
        }
        if (finalExpression == null)
        {
            return null;
        }
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        return await dbSet.Where(lambda).FirstOrDefaultAsync();
    }
    public T? findRow<T>(params (string propertyName, object value)[] conditions) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression? finalExpression = null;
        foreach (var (propertyName, value) in conditions)
        {
            MemberExpression property = Expression.Property(parameter, propertyName);
            BinaryExpression condition = Expression.Equal(property, Expression.Constant(value));

            finalExpression = finalExpression == null ? condition : Expression.AndAlso(finalExpression, condition);
        }
        if (finalExpression == null)
        {
            return null;
        }
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        return dbSet.Where(lambda).FirstOrDefault();
    }
    /*----------*/
    public async Task<List<T?>> FindRowsAsync<T>(params (string propertyName, object value)[] conditions) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression? finalExpression = null;
        foreach (var (propertyName, value) in conditions)
        {
            MemberExpression property = Expression.Property(parameter, propertyName);
            BinaryExpression condition = Expression.Equal(property, Expression.Constant(value));

            finalExpression = finalExpression == null ? condition : Expression.AndAlso(finalExpression, condition);
        }
        if (finalExpression == null)
        {
            return null;
        }
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        return await dbSet.Where(lambda).ToListAsync();
    }
    public List<T?> FindRows<T>(params (string propertyName, object value)[] conditions) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression? finalExpression = null;
        foreach (var (propertyName, value) in conditions)
        {
            MemberExpression property = Expression.Property(parameter, propertyName);
            BinaryExpression condition = Expression.Equal(property, Expression.Constant(value));

            finalExpression = finalExpression == null ? condition : Expression.AndAlso(finalExpression, condition);
        }
        if (finalExpression == null)
        {
            return null;
        }
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        return [.. dbSet.Where(lambda)];
    }
    /*----------*/
    public List<T?> ListRow<T>(bool onlyView=false) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();

        return onlyView ? [.. dbSet.AsNoTracking()] : [.. dbSet];
    }
    public async Task<List<T?>> ListRowAsync<T>(bool onlyView = false) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();

        return onlyView ? await dbSet.AsNoTracking().ToListAsync() : await dbSet.ToListAsync();
    }
    /*----------*/
    public async Task<bool> AnyRowAsync<T>(params (string propertyName, object value)[] conditions) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression? finalExpression = null;
        foreach (var (propertyName, value) in conditions)
        {
            MemberExpression property = Expression.Property(parameter, propertyName);
            BinaryExpression condition = Expression.Equal(property, Expression.Constant(value));

            finalExpression = finalExpression == null ? condition : Expression.AndAlso(finalExpression, condition);
        }
        if (finalExpression == null)
        {
            return false;
        }
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        return await dbSet.AnyAsync(lambda);
    }
    public bool AnyRow<T>(params (string propertyName, object value)[] conditions) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression? finalExpression = null;
        foreach (var (propertyName, value) in conditions)
        {
            MemberExpression property = Expression.Property(parameter, propertyName);
            BinaryExpression condition = Expression.Equal(property, Expression.Constant(value));

            finalExpression = finalExpression == null ? condition : Expression.AndAlso(finalExpression, condition);
        }
        if (finalExpression == null)
        {
            return false;
        }
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        return dbSet.Any(lambda);
    }
    /*----------*/
    public int? RowCount<T>(params (string propertyName, object value)[] conditions) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
        Expression? finalExpression = null;
        foreach (var (propertyName, value) in conditions)
        {
            MemberExpression property = Expression.Property(parameter, propertyName);
            BinaryExpression condition = Expression.Equal(property, Expression.Constant(value));

            finalExpression = finalExpression == null ? condition : Expression.AndAlso(finalExpression, condition);
        }
        if (finalExpression == null)
        {
            return null;
        }
        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        return dbSet.Count(lambda);
    }
    /*----------*/
    public EntityEntry<T> AddRow<T>(T entity) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();
        EntityEntry<T> entry=dbSet.Add(entity);        
        return entry;
    }
    public async Task<EntityEntry<T>> AddRowAsync<T>(T entity) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();
        EntityEntry<T> entry = await dbSet.AddAsync(entity);
        return entry;
    }
    /*----------*/
    public EntityEntry<T> DeleteRow<T>(T entity) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();
        EntityEntry<T> entry = dbSet.Remove(entity);
        return entry;
    }
    public void DeleteRows<T>(IEnumerable<T> entity) where T : class, IEntity
    {
        DbSet<T> dbSet = Db.Set<T>();
        dbSet.RemoveRange(entity);
    }
    /*----------*/
    public void DeleteFile(string path)
    {
        string deletePath = Path.Combine(Environment.CurrentDirectory, UploadImageDirectory, path ?? "");
        if (File.Exists(deletePath))
        {
            File.Delete(deletePath);
        }
    }
    public async Task UploadImageFile(IFormFile file, string fileName)
    {
        string path = Path.Combine(Environment.CurrentDirectory, UploadImageDirectory, fileName);
        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);
    }
    public async Task UploadFile(IFormFile file, string fileName,string directory)
    {
        string path = Path.Combine(Environment.CurrentDirectory, directory, fileName);
        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);
    }
    /*----------*/

    public void Dispose()
    {
        if (Db != null)
        {
            Db.Dispose();
            Db = null;
        }

        GC.SuppressFinalize(this);
    }
}