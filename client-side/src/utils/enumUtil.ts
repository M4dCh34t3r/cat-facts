/**
 * Converts a TypeScript enum into an array of objects containing numeric values and their corresponding enum.
 *
 * @summary This function processes a TypeScript enum and extracts numeric values,
 * returning them as an array of objects with the original enum reference and the value.
 *
 * @template T - A generic type extending an object, expected to be a TypeScript enum.
 * @param {T} e - The TypeScript enum to convert.
 * @returns {{ key: string; value: number }[]} An array of objects containing the enum reference and numeric values.
 *
 * @example
 * enum ExampleEnum {
 *   A = 1,
 *   B = 2,
 *   C = 3
 * }
 * const result = enumToObject(ExampleEnum);
 * console.log(result);
 * // Output:
 * // [
 * //   { key: 'A', value: 1 },
 * //   { key: 'B', value: 2 },
 * //   { key: 'C', value: 3 }
 * // ]
 */
export function enumToObject<T extends object>(e: T): { key: string; value: number }[] {
  const renameKey = (key: string) => {
    key = key.replace(/([a-z])([A-Z])/g, '$1 $2');
    return key.charAt(0).toUpperCase() + key.slice(1).toLowerCase();
  };
  return Object.values(e)
    .filter((value) => typeof value === 'number')
    .map((value: number) => ({
      key: renameKey((e as T[])[value] as unknown as string),
      value
    }));
}
