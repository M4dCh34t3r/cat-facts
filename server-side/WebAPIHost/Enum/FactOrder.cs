namespace WebAPIHost.Enum;

/// <summary>
/// Represents different ordering options for facts.
/// </summary>
public enum FactOrder
{
    /// <summary>
    /// Orders facts alphabetically.
    /// </summary>
    Alphabetical,

    /// <summary>
    /// Orders facts based on their insertion date and time.
    /// </summary>
    Insertion,

    /// <summary>
    /// Orders facts based on their occurrence (e.g., frequency or count).
    /// </summary>
    Occurrence,

    /// <summary>
    /// Orders facts based on how much they are liked.
    /// </summary>
    Like,

    /// <summary>
    /// Orders facts based on how much they are disliked.
    /// </summary>
    Dislike,

    /// <summary>
    /// Orders facts based on popularity, which is determined by the difference between likes and dislikes.
    /// </summary>
    Popularity,
}
