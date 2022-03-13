export const makeLocalStorageService = () => ({
  get: <T>(key: string): T | undefined => {
    try {
      const value = localStorage.getItem(key);

      return value ? (JSON.parse(value) as T) : void 0;
    } catch (e) {
      return void 0;
    }
  },
  set: <T>(key: string, value: T) => {
    localStorage.setItem(key, JSON.stringify(value));
  },
  delete: (key: string) => localStorage.removeItem(key),
});

export const lcManager = makeLocalStorageService();
