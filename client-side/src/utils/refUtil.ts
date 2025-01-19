import { customRef, type Ref } from 'vue';

/**
 * Creates a custom Vue `ref` synchronized with `localStorage`.
 *
 * @summary This function generates a custom Vue `ref` that reads and writes
 * data to `localStorage`, allowing reactive state to persist across page reloads.
 *
 * @param key - The key under which the value will be stored in `localStorage`.
 * @param value - The default value to return if no entry is found in `localStorage`.
 * @returns {Ref<T>} A Vue `ref` object synchronized with `localStorage`.
 *
 * @example
 * // Create a ref synchronized with localStorage
 * const username = localRef<string>('username', 'Guest');
 * // Reading the value (reactive)
 * console.log(username.value); // 'Guest' (or the saved value in localStorage)
 * // Updating the value
 * username.value = 'Alice'; // Saves 'Alice' to localStorage under the key 'username'
 * // Removing the value
 * username.value = undefined; // Removes the key 'username' from localStorage
 */
export function localRef<T = undefined>(key: string, value?: T): Ref<T> {
  return customRef<T>((track, trigger) => ({
    get: () => {
      track();
      const val = localStorage.getItem(key);
      return val ? JSON.parse(val) : value;
    },
    set: (val) => {
      val ? localStorage.setItem(key, JSON.stringify(val)) : localStorage.removeItem(key);
      trigger();
    }
  }));
}

/**
 * Creates a custom Vue `ref` synchronized with `sessionStorage`.
 *
 * @summary This function generates a custom Vue `ref` that reads and writes
 * data to `sessionStorage`, allowing reactive state to persist across page reloads.
 *
 * @param key - The key under which the value will be stored in `sessionStorage`.
 * @param value - The default value to return if no entry is found in `sessionStorage`.
 * @returns {Ref<T>} A Vue `ref` object synchronized with `sessionStorage`.
 *
 * @example
 * // Create a ref synchronized with sessionStorage
 * const username = localRef<string>('username', 'Guest');
 * // Reading the value (reactive)
 * console.log(username.value); // 'Guest' (or the saved value in sessionStorage)
 * // Updating the value
 * username.value = 'Alice'; // Saves 'Alice' to sessionStorage under the key 'username'
 * // Removing the value
 * username.value = undefined; // Removes the key 'username' from sessionStorage
 */
export function sessionRef<T = undefined>(key: string, value?: T): Ref<T> {
  return customRef<T>((track, trigger) => ({
    get: () => {
      track();
      const val = sessionStorage.getItem(key);
      return val ? JSON.parse(val) : value;
    },
    set: (val) => {
      val ? sessionStorage.setItem(key, JSON.stringify(val)) : sessionStorage.removeItem(key);
      trigger();
    }
  }));
}
