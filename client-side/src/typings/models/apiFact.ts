/**
 * Represents a cat fact for an API.
 */
export interface APIFact {
  /**
   * The unique identifier for the cat fact.
   */
  id: string;

  /**
   * The cat fact text.
   */
  text: string;

  /**
   * The fact insertion date and time.
   */
  insertedAt: Date;

  /**
   * The fact occurrence count.
   */
  occurrenceCount: number;

  /**
   * The fact like count.
   */
  likeCount: number;

  /**
   * The fact dislike count.
   */
  dislikeCount: number;
}
