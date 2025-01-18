namespace Core.Domain.Entities;

/// <summary>
/// Represents a publicly accessible fact about cats.
/// </summary>
public class PublicFact
{
    /// <summary>
    /// Gets or sets the cat fact data.
    /// </summary>
    public string[] Data { get; set; } = [];
}
