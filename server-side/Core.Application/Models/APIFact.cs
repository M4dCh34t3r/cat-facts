using System.ComponentModel.DataAnnotations;

namespace Core.Application.Models;

/// <summary>
/// Represents a cat fact for an API.
/// </summary>
public class APIFact
{
    /// <summary>
    /// Gets or sets the unique identifier for the cat fact.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the cat fact text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the fact insertion date and time.
    /// </summary>
    public DateTime InsertedAt { get; set; }

    /// <summary>
    /// Gets or sets the fact occurrence count.
    /// </summary>
    public int OccurrenceCount { get; set; } = 0;

    /// <summary>
    /// Gets or sets the fact like count.
    /// </summary>
    public int LikeCount { get; set; } = 0;

    /// <summary>
    /// Gets or sets the fact dislike count.
    /// </summary>
    public int DislikeCount { get; set; } = 0;
}
