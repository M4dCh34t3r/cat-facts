using Core.Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UpsertRepository(AppDbContext context) : IUpsertRepository
{
    private readonly Serilog.ILogger _logger = Serilog.Log.ForContext<UpsertRepository>();

    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Fact>> UpsertAsync(PublicFact publicFact, string requestURI)
    {
        var facts = TransformFact(publicFact, requestURI).ToArray();

        if (facts.Length == 0)
        {
            _logger.Debug("No facts to persist.");
            return facts;
        }

        var factTexts = facts.Select(f => f.Text).ToHashSet();

        var existingFacts = await _context
            .Facts.Where(f => factTexts.Contains(f.Text))
            .ToListAsync();

        foreach (var existingFact in existingFacts)
        {
            existingFact.OccurrenceCount++;
            _context.Facts.Update(existingFact);
        }

        var newFacts = facts.Where(f => !existingFacts.Any(ef => ef.Text == f.Text)).ToList();

        if (newFacts.Count > 0)
            await _context.Facts.AddRangeAsync(newFacts);

        var persistedCount = await _context.SaveChangesAsync();
        _logger.Debug(
            "Successfully persisted {PersistedCount} fact(s) (updated or inserted) into the database.",
            persistedCount
        );
        return facts;
    }

    private static IEnumerable<Fact> TransformFact(PublicFact publicFact, string requestURI) =>
        publicFact.Data.Select(d => new Fact
        {
            Source = requestURI,
            Text = d.Trim(),
        });
}
