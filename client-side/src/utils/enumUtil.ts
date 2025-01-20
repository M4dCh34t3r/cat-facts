/**
 * Converts a TypeScript enum into an array of objects, each object contains a enum key and its corresponding value.

 * @template T - The type of the enum.
 * @param enumObj - The TypeScript enum to convert.
 * @returns An array of objects, each containing a 'key' and 'value' property.
 *
 * @example
 * enum ExampleEnum {
 *   A = 1,
 *   B = 2,
 *   C = 3
 * }
 * 
 * const enumObjects = enumToObject(ExampleEnum);
 * 
 * console.log(enumObjects);
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
