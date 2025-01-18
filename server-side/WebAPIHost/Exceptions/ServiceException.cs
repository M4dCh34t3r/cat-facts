using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;
using WebAPIHost.DTOs;
using WebAPIHost.Enum;

namespace WebAPIHost.Exceptions;

/// <summary>
/// Represents a custom exception for handling service layer occurrences.
/// </summary>
/// <param name="statusCode">The HTTP status code associated with the exception.</param>
/// <param name="category">The category of the exception, indicating its severity or type. Defaults to <see cref="ServiceExceptionCategory.Ignore"/>.</param>
/// <param name="title">A short, descriptive title for the exception.</param>
/// <param name="text">A detailed message providing additional context for the exception.</param>
public class ServiceException(
    HttpStatusCode statusCode,
    ServiceExceptionCategory category = ServiceExceptionCategory.Ignore,
    string title = "",
    string text = ""
) : Exception
{
    /// <summary>
    /// Gets the HTTP status code associated with the exception.
    /// </summary>
    public readonly HttpStatusCode StatusCode = statusCode;

    /// <summary>
    /// Gets the category of the exception, indicating its severity or type.
    /// </summary>
    public readonly ServiceExceptionCategory Category = category;

    /// <summary>
    /// Gets the short, descriptive title for the exception.
    /// </summary>
    public readonly string Title = title;

    /// <summary>
    /// Gets the detailed message providing additional context for the exception.
    /// </summary>
    public readonly string Text = text;

    /// <summary>
    /// Converts the exception to an <see cref="IActionResult"/> with the appropriate status code.
    /// </summary>
    /// <returns>An <see cref="ObjectResult"/> containing the exception details and status code.</returns>
    public IActionResult ToObjectResult() =>
        new ObjectResult(ToDTO()) { StatusCode = (int)StatusCode };

    /// <summary>
    /// Serializes the exception details to a JSON string.
    /// </summary>
    /// <returns>A JSON string representing the exception details.</returns>
    public override string ToString() => JsonUtil.Serialize(ToDTO());

    /// <summary>
    /// Converts the exception to it's DTO counterpart.
    /// </summary>
    /// <returns>An object with the <see cref="Title"/>, <see cref="Text"/>, and <see cref="Category"/> properties.</returns>
    private ServiceExceptionDTO ToDTO() => new(Category, Title, Text);
}
