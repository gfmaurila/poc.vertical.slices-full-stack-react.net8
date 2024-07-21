using Microsoft.EntityFrameworkCore;
using poc.admin.Domain;
using poc.admin.Domain.User;
using poc.admin.Infrastructure.Database.Mappings;

namespace poc.admin.Infrastructure.Database;

public class EFSqlServerContext : DbContext
{
    public EFSqlServerContext()
    { }

    public EFSqlServerContext(DbContextOptions<EFSqlServerContext> options) : base(options)
    { }

    public virtual DbSet<Article> Article { get; set; }

    public virtual DbSet<UserEntity> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);
        base.OnConfiguring(optionsBuilder);
    }

    public static readonly LoggerFactory _loggerFactory = new LoggerFactory(new[] {
        new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
    });
}
