using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly AppDbContext _db;

    public UrlShortenerService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ShortenUrlResponse> ShortenAsync(ShortenUrlRequest request, string baseUrl)
    {
        var alias = string.IsNullOrWhiteSpace(request.CustomAlias)
            ? GenerateAlias()
            : request.CustomAlias.Trim();

        if (!IsValidAlias(alias))
            throw new ArgumentException("Alias may only contain letters, numbers, and hyphens (2–64 characters).");

        var exists = await _db.ShortenedUrls.AnyAsync(u => u.Alias == alias);
        if (exists)
            throw new InvalidOperationException($"The alias '{alias}' is already taken.");

        var entry = new ShortenedUrl
        {
            Alias = alias,
            FullUrl = request.FullUrl
        };

        _db.ShortenedUrls.Add(entry);
        await _db.SaveChangesAsync();

        return new ShortenUrlResponse
        {
            Alias = alias,
            FullUrl = request.FullUrl,
            ShortUrl = $"{baseUrl}/{alias}"
        };
    }

    public async Task<string?> GetFullUrlAsync(string alias)
    {
        var entry = await _db.ShortenedUrls
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Alias == alias);

        return entry?.FullUrl;
    }

    public async Task<IEnumerable<UrlListItem>> GetAllAsync(string baseUrl)
    {
        return await _db.ShortenedUrls
            .AsNoTracking()
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new UrlListItem
            {
                Alias = u.Alias,
                FullUrl = u.FullUrl,
                ShortUrl = $"{baseUrl}/{u.Alias}"
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(string alias)
    {
        var entry = await _db.ShortenedUrls.FirstOrDefaultAsync(u => u.Alias == alias);
        if (entry is null)
            return false;

        _db.ShortenedUrls.Remove(entry);
        await _db.SaveChangesAsync();
        return true;
    }

    // --- Private helpers ---

    private static string GenerateAlias()
    {
        // 7-character Base62 alias: enough entropy without being unwieldy
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable
            .Range(0, 7)
            .Select(_ => chars[Random.Shared.Next(chars.Length)])
            .ToArray());
    }

    private static bool IsValidAlias(string alias)
    {
        if (alias.Length < 2 || alias.Length > 64)
            return false;

        return alias.All(c => char.IsLetterOrDigit(c) || c == '-');
    }
}
