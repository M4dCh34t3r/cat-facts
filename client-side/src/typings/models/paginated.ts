/**
 * Represents a paginated list of items.
 * @template T The type of the items in the list.
 */
export interface Paginated<T> {
  /**
   * The current page items.
   */
  items: T[];

  /**
   * The current page index (zero-based).
   */
  pageIndex: number;

  /**
   * The current page size.
   */
  pageSize: number;

  /**
   * The total number of items across all pages.
   */
  totalItems: number;

  /**
   * The total number of pages.
   */
  totalPages: number;
}
