namespace WebAPIHost.Enum;

/// <summary>
/// Represents a service exception category.
/// </summary>
public enum ServiceExceptionCategory
{
    /// <summary>
    /// Indicates that the exception should be ignored and does not require further action.
    /// </summary>
    Ignore,

    /// <summary>
    /// Indicates a warning exception that does not prevent normal operations but may require attention.
    /// </summary>
    Warning,

    /// <summary>
    /// Indicates an error exception that prevents the service from completing its operation.
    /// </summary>
    Error,

    /// <summary>
    /// Indicates an informational exception, often used for logging or providing details to the user.
    /// </summary>
    Information,

    /// <summary>
    /// Indicates a successful operation, typically used for status reporting.
    /// </summary>
    Success,
}
