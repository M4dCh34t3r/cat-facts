using System.Net;
using Core.Application.Models;
using Core.Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using WebAPIHost.Enum;
using WebAPIHost.Exceptions;

namespace WebAPIHost.Services;

public class FactService(AppDbContext context, IConfiguration configuration) : IFactService
{
    private readonly AppDbContext _context = context;

    private readonly IConfiguration _configuration = configuration;

    public async Task<Paginated<APIFact>> ReadAsync(FactOrder order, bool descending, int pageIndex)
    {
        Func<IQueryable<Fact>, IOrderedQueryable<Fact>> orderByFunc = order switch
        {
            FactOrder.Alphabetical => descending
                ? (q => q.OrderByDescending(f => f.Text))
                : (q => q.OrderBy(f => f.Text)),

            FactOrder.Insertion => descending
                ? (q => q.OrderByDescending(f => f.InsertedAt))
                : (q => q.OrderBy(f => f.InsertedAt)),

            FactOrder.Occurrence => descending
                ? (q => q.OrderByDescending(f => f.OccurrenceCount))
                : (q => q.OrderBy(f => f.OccurrenceCount)),

            FactOrder.Like => descending
                ? (q => q.OrderByDescending(f => f.LikeCount))
                : (q => q.OrderBy(f => f.LikeCount)),

            FactOrder.Dislike => descending
                ? (q => q.OrderByDescending(f => f.DislikeCount))
                : (q => q.OrderBy(f => f.DislikeCount)),

            _ => descending
                ? (q => q.OrderByDescending(f => f.LikeCount - f.DislikeCount))
                : (q => q.OrderBy(f => f.LikeCount - f.DislikeCount)),
        };

        var facts = _context.Facts.AsQueryable();

        if (!facts.Any())
            throw new ServiceException(
                HttpStatusCode.NotFound,
                ServiceExceptionCategory.Information,
                "No facts found",
                "There are no facts in the app"
            );

        int totalItems = await facts.CountAsync();
        int pageSize = _configuration.GetValue<int>("Misc:PageSize");
        var items = await orderByFunc(_context.Facts.AsQueryable())
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(f => new APIFact
            {
                OccurrenceCount = f.OccurrenceCount,
                DislikeCount = f.DislikeCount,
                InsertedAt = f.InsertedAt,
                LikeCount = f.LikeCount,
                Text = f.Text,
                Id = f.Id,
            })
            .ToListAsync();

        Paginated<APIFact> paginated = new(items, pageIndex, pageSize, totalItems);
        return paginated;
    }

    public async Task<APIFact> UpdateDislikeCountByIdAsync(Guid id)
    {
        var fact = await ReadByIdAsync(id);
        fact.DislikeCount++;
        await _context.SaveChangesAsync();
        return GenerateAPIFact(fact);
    }

    public async Task<APIFact> UpdateLikeCountByIdAsync(Guid id)
    {
        var fact = await ReadByIdAsync(id);
        fact.LikeCount++;
        await _context.SaveChangesAsync();
        return GenerateAPIFact(fact);
    }

    private async Task<Fact> ReadByIdAsync(Guid id)
    {
        if (await _context.Facts.FindAsync(id) is not Fact fact)
            throw new ServiceException(
                HttpStatusCode.NotFound,
                ServiceExceptionCategory.Information,
                "No fact found",
                "The specified fact could not be found"
            );

        return fact;
    }

    private static APIFact GenerateAPIFact(Fact fact) =>
        new()
        {
            OccurrenceCount = fact.OccurrenceCount,
            DislikeCount = fact.DislikeCount,
            InsertedAt = fact.InsertedAt,
            LikeCount = fact.LikeCount,
            Text = fact.Text,
            Id = fact.Id,
        };
}
