export const createStubFactory = <T>(defaultDataFactory: () => T) => {
  return (data: Partial<T> = {}) => ({ ...defaultDataFactory(), ...data } as T);
};
