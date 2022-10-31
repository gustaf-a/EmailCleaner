using Microsoft.EntityFrameworkCore;

namespace MailProviderService.Data;

public class MailProviderServiceContext : DbContext
{
	public MailProviderServiceContext(DbContextOptions<MailProviderServiceContext> options)
		: base(options) {}

	public DbSet<Email> Email { get; set; }
}
