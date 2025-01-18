using WebAPIHost.Enum;

namespace WebAPIHost.DTOs;

/// <summary>
/// Represents a Data Transfer Object (DTO) for conveying details of a service exception.
/// </summary>
/// <param name="category">The category of the exception DTO, indicating its severity or type. Defaults to <see cref="ServiceExceptionCategory.Ignore"/>.</param>
/// <param name="title">A short, descriptive title for the exception DTO.</param>
/// <param name="text">A detailed message providing additional context for the exception DTO.</param>
public class ServiceExceptionDTO(
    ServiceExceptionCategory category = ServiceExceptionCategory.Ignore,
    string title = "",
    string text = ""
)
{
    /// <summary>
    /// Gets the category of the DTO, indicating its severity or type.
    /// </summary>
    public ServiceExceptionCategory Category { get; set; } = category;

    /// <summary>
    /// Gets the short, descriptive title for the DTO.
    /// </summary>
    public string Title { get; set; } = title;

    /// <summary>
    /// Gets the detailed message providing additional context for the DTO.
    /// </summary>
    public string Text { get; set; } = text;
}
