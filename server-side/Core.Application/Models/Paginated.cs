namespace Core.Application.Models;

/// <summary>
/// A paginated enumerable of items.
/// </summary>
/// <typeparam name="T">The type of the items in the list.</typeparam>
public class Paginated<T>(IEnumerable<T> items, int pageIndex, int pageSize, int totalItens)
{
    /// <summary>
    /// The current page items.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = items;

    /// <summary>
    /// The current page index.
    /// </summary>
    public int PageIndex { get; set; } = pageIndex;

    /// <summary>
    /// The current page size.
    /// </summary>
    public int PageSize { get; set; } = pageSize;

    /// <summary>
    /// The total number of items across all pages.
    /// </summary>
    public int TotalItems { get; set; } = totalItens;

    /// <summary>
    /// The total number of pages.
    /// </summary>
    public int TotalPages { get; set; } = (int)Math.Ceiling(totalItens / (double)pageSize);
}
