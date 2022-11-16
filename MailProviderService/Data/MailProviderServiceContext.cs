using Microsoft.EntityFrameworkCore;

namespace MailProviderService.Data;

public class MailProviderServiceContext : DbContext
{
	public MailProviderServiceContext(DbContextOptions<MailProviderServiceContext> options)
		: base(options) 
	{
    }

    public DbSet<Email> Emails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Email>().ToTable(nameof(Emails));
    }
}
