using Core.Application.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPIHost.DTOs;
using WebAPIHost.Enum;
using WebAPIHost.Services;

namespace WebAPIHost.Controllers;

public class FactController(IFactService service) : ApiController
{
    private readonly IFactService _service = service;

    /// <summary>
    /// Asynchronously retrieves a collection of facts with the specified order, descending flag, and pagination.
    /// </summary>
    /// <param name="order">The order by which the facts should be sorted (e.g., Alphabetical, Insertion, Occurrence, etc.).</param>
    /// <param name="descending">A flag indicating whether the results should be in descending order.</param>
    /// <param name="pageIndex">The page index for pagination to retrieve the corresponding set of facts.</param>
    /// <response code="200">Indicates that the Facts ware successfully retrieved.</response>
    /// <response code="404">Indicates that no Fact was found.</response>
    /// <response code="500">Indicates that an internal server error has occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(Paginated<APIFact>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServiceExceptionDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(FactOrder order, bool descending, int pageIndex) =>
        await ExecuteAsync(async () => Ok(await _service.ReadAsync(order, descending, pageIndex)));

    /// <summary>
    /// Asynchronously "dislikes" a fact by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the fact to dislike.</param>
    /// <response code="200">Indicates that the Fact was successfully disliked.</response>
    /// <response code="404">Indicates that the Fact with the specified ID was not found.</response>
    /// <response code="500">Indicates that an internal server error has occurred.</response>
    [HttpPost("{id}/Dislike")]
    [ProducesResponseType(typeof(APIFact), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServiceExceptionDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostIdDislikeAsync(Guid id) =>
        await ExecuteAsync(async () => Ok(await _service.UpdateDislikeCountByIdAsync(id)));

    /// <summary>
    /// Asynchronously "likes" a fact by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the fact to like.</param>
    /// <response code="200">Indicates that the Fact was successfully liked.</response>
    /// <response code="404">Indicates that the Fact with the specified ID was not found.</response>
    /// <response code="500">Indicates that an internal server error has occurred.</response>
    [HttpPost("{id}/Like")]
    [ProducesResponseType(typeof(APIFact), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServiceExceptionDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostIiLikeAsync(Guid id) =>
        await ExecuteAsync(async () => Ok(await _service.UpdateLikeCountByIdAsync(id)));
}
