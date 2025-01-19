/**
 * Represents a service exception category.
 */
export enum FactOrder {
  /**
   * Orders facts alphabetically.
   */
  Alphabetical,
  /**
   * Order facts based on their insertion date and time
   */
  Insertion,
  /**
   * Orders facts based on their occurrence (e.g., frequency or count).
   */
  Occurrence,
  /**
   * Orders facts based on how much they are liked.
   */
  Like,
  /**
   * Orders facts based on how much they are disliked.
   */
  Dislike,
  /**
   * Orders facts based on popularity, which is determined by the difference between likes and dislikes.
   */
  Popularity
}
