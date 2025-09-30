using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Seeding;

public class FileSeeder
{
    private readonly AppDbContext _appDbContext;

    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public FileSeeder(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task SeedAllJsonOnceAsync(string? runOnceKey, string? profPath, string? studPath, string? profStudPath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(runOnceKey))
            return;

        if (await _appDbContext.SeedHistories.AsNoTracking().AnyAsync(x => x.Key == runOnceKey, ct))
            return;

        var profs = await ReadJsonAsync<Professor>(profPath, ct);
        var studs = await ReadJsonAsync<Student>(studPath, ct);
        var links = await ReadJsonAsync<ProfessorStudent>(profStudPath, ct);

        await using var transaction = await _appDbContext.Database.BeginTransactionAsync(ct);

        try {

            if (profs.Count > 0)
                await _appDbContext.Professors.AddRangeAsync(profs, ct);

            if (studs.Count > 0)
                await _appDbContext.Students.AddRangeAsync(studs, ct);

            if (profs.Count > 0 || studs.Count > 0)
                await _appDbContext.SaveChangesAsync(ct);

            if (links.Count > 0)
            {
                var profList = await _appDbContext.Professors
                    .AsNoTracking()
                    .Select(x => x.Id)
                    .ToListAsync(ct);

                var studList = await _appDbContext.Students
                    .AsNoTracking()
                    .Select(x => x.Id)
                    .ToListAsync(ct);

                var profIds = profList.ToHashSet();
                var studIds = studList.ToHashSet();

                var unique = new HashSet<(int p, int s)>();
                var cleaned = new List<ProfessorStudent>();

                foreach (var l in links)
                {
                    if (profIds.Contains(l.ProfessorId) && studIds.Contains(l.StudentId))
                    {
                        var key = (l.ProfessorId, l.StudentId);
                        if (unique.Add(key))
                            cleaned.Add(l);
                    }
                }

                if (cleaned.Count > 0)
                    await _appDbContext.ProfessorStudents.AddRangeAsync(cleaned, ct);
            }

            _appDbContext.SeedHistories.Add(new SeedHistory { Key = runOnceKey });
            await _appDbContext.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);
        }

        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            throw ex;
        }
    }

    private static async Task<List<T>> ReadJsonAsync<T>(string? path, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return new List<T>();

        await using var fs = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<List<T>>(fs, _json, ct) ?? new List<T>();
    }
}