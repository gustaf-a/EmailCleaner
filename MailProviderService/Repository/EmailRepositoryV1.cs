using MailProviderService.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MailProviderService.Repository;

public class EmailRepositoryV1 : IEmailRepository
{
    private MailProviderServiceContext _context;

    public EmailRepositoryV1(MailProviderServiceContext context)
    {
        _context = context;
    }

    public async Task Add(List<Email> emails)
    {
        if (emails is null || emails.Count == 0)
            return;

        Log.Information($"Adding {emails.Count} emails to database.");

        try
        {
            _context.Email.AddRange(emails);

            await _context.SaveChangesAsync();

            Log.Information($"Added {emails.Count} emails successfully to database.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to add emails to database.");
            throw;
        }

        return;
    }

    public async Task Delete(List<Email> emails)
    {
        Log.Information($"Removing {emails.Count} emails.");

        _context.RemoveRange(emails);

        await _context.SaveChangesAsync();

        Log.Information($"Removed {emails.Count} emails.");

        return;
    }

    public async Task<List<Email>> GetAll()
    {
        var emails = await _context.Email.ToListAsync();

        if (emails is null)
        {
            Log.Warning("Email context returning null when trying to access all emails.");
            emails = new();
        }

        Log.Information($"Returning {emails.Count} emails");

        return emails;
    }
}
