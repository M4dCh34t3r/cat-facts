import type { ServiceExceptionCategory } from '@/enums';

/**
 * Represents a Data Transfer Object (DTO) for conveying details of a service exception.
 */
export interface ServiceExceptionDTO {
  /**
   * The category of the exception DTO, indicating its severity or type.
   */
  category: ServiceExceptionCategory;

  /**
   * A short, descriptive title for the exception DTO.
   */
  title: string;

  /**
   * A detailed message providing additional context for the exception DTO.
   */
  text: string;
}
