/**
 * Represents a service exception category.
 */
export enum ServiceExceptionCategory {
  /**
   * Indicates that the exception should be ignored and does not require further action.
   */
  Ignore = 'Ignore',

  /**
   * Indicates a warning exception that does not prevent normal operations but may require attention.
   */
  Warning = 'Warning',

  /**
   * Indicates an error exception that prevents the service from completing its operation.
   */
  Error = 'Error',

  /**
   * Indicates an informational exception, often used for logging or providing details to the user.
   */
  Information = 'Information',

  /**
   * Indicates a successful operation, typically used for status reporting.
   */
  Success = 'Success'
}
