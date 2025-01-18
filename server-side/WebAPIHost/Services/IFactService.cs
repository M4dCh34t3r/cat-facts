using Core.Application.Models;
using WebAPIHost.Enum;

namespace WebAPIHost.Services;

public interface IFactService
{
    /// <summary>
    /// Asynchronously retrieves a paginated collection of facts with the specified order and descending flag.
    /// </summary>
    /// <param name="order">The order by which the facts should be sorted (e.g., Alphabetical, Insertion, Occurrence, etc.).</param>
    /// <param name="descending">A flag indicating whether the results should be in descending order.</param>
    /// <param name="pageIndex">The page number for pagination to retrieve the corresponding set of facts.</param>
    /// <returns>A task representing the asynchronous operation, with a collection of <see cref="APIFact"/> as the result.</returns>
    Task<Paginated<APIFact>> ReadAsync(FactOrder order, bool descending, int pageIndex);

    /// <summary>
    /// Asynchronously "dislikes" a fact by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the fact to dislike.</param>
    /// <returns>A task representing the asynchronous operation, with the corresponding <see cref="APIFact"/> as the result.</returns>
    Task<APIFact> UpdateDislikeCountByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously "likes" a fact by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the fact to like.</param>
    /// <returns>A task representing the asynchronous operation, with the corresponding <see cref="APIFact"/> as the result.</returns>
    Task<APIFact> UpdateLikeCountByIdAsync(Guid id);
}
