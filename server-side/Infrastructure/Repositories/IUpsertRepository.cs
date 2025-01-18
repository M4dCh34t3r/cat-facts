using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public interface IUpsertRepository
{
    /// <summary>
    /// Inserts and updates multiple facts in the repository based on the provided public fact data.
    /// </summary>
    /// <param name="publicFact">The public fact data to upsert.</param>
    /// <param name="requestURI">The request uri from where the data was fetched.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the upserted facts.
    /// </returns>
    /// <exception cref="DbUpdateException">Thrown if the specified "Text" is conflicting with an already existing entry.</exception>
    Task<IEnumerable<Fact>> UpsertAsync(PublicFact publicFact, string requestURI);
}
