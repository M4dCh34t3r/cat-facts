import { customRef, type Ref } from 'vue';

/**
 * Generates a custom Vue `ref` that reads and writes data to `localStorage`, enabling persistent reactive state across page reloads.
 *
 * @template T - The type of the value stored in the ref.
 * @param key - The key under which the value will be stored in `localStorage`.
 * @param defaultValue - The default value to return if no entry is found in `localStorage`.
 * @returns A Vue `ref` object synchronized with `localStorage`.
 *
 * @example
 * const username = useLocalStorageRef<string>('username', 'Guest');
 * // Accessing the value (reactive)
 * console.log(username.value);
 * // Updating the value
 * username.value = 'Alice';
 * // Removing the value
 * username.value = undefined;
 */
export function useLocalStorageRef<T = undefined>(key: string, defaultValue?: T): Ref<T> {
  return customRef<T>((track, trigger) => ({
    get: () => {
      track();
      const storedValue = localStorage.getItem(key);
      return storedValue ? JSON.parse(storedValue) : defaultValue;
    },
    set: (newValue) => {
      if (newValue === undefined) localStorage.removeItem(key);
      else localStorage.setItem(key, JSON.stringify(newValue));
      trigger();
    }
  }));
}

/**
 * Generates a custom Vue `ref` that reads and writes data to `sessionStorage`, enabling persistent reactive state across page reloads.
 *
 * @template T - The type of the value stored in the ref.
 * @param key - The key under which the value will be stored in `sessionStorage`.
 * @param defaultValue - The default value to return if no entry is found in `sessionStorage`.
 * @returns A Vue `ref` object synchronized with `sessionStorage`.
 *
 * @example
 * const username = useSessionStorageRef<string>('username', 'Guest');
 * // Accessing the value (reactive)
 * console.log(username.value);
 * // Updating the value
 * username.value = 'Alice';
 * // Removing the value
 * username.value = undefined;
 */
export function useSessionStorageRef<T = undefined>(key: string, defaultValue?: T): Ref<T> {
  return customRef<T>((track, trigger) => ({
    get: () => {
      track();
      const storedValue = sessionStorage.getItem(key);
      return storedValue ? JSON.parse(storedValue) : defaultValue;
    },
    set: (newValue) => {
      if (newValue === undefined) sessionStorage.removeItem(key);
      else sessionStorage.setItem(key, JSON.stringify(newValue));
      trigger();
    }
  }));
}
