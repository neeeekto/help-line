import { ArgumentTypes, FunctionType } from "@core/types";

type MockFn<
  TStore,
  TKey extends keyof TStore,
  TStoreItem extends FunctionType<
    ArgumentTypes<TStoreItem>,
    ReturnType<TStoreItem>
  >
> = (
  mock: jest.Mock<ReturnType<TStoreItem>, ArgumentTypes<TStoreItem>>
) => jest.Mock<ReturnType<TStoreItem>, ArgumentTypes<TStoreItem>>;

type MockMethodsType<TStore> = Partial<{
  [P in keyof TStore]: TStore[P] extends FunctionType
    ? MockFn<TStore, P, TStore[P]>
    : never;
}>;

type MockStateType<TStore> = Partial<{
  [P in keyof TStore]: TStore[P] extends FunctionType ? never : TStore[P];
}>;

export const mockStore = <TStore>(
  store: TStore,
  mocks?: MockMethodsType<TStore>,
  state?: MockStateType<TStore>
) => {
  const newStore: any = { ...store };
  const keys = Object.keys(mocks || {});
  const mockResult: any = {};
  for (const key of keys) {
    const mockSetter = (mocks as any)[key];
    const jestMock = jest.fn();
    const mock = mockSetter(jestMock);
    newStore[key] = mock;
    mockResult[key] = mock;
  }
  for (const key of Object.keys(state || {})) {
    const stateMock = (state as any)[key];
    if (stateMock) {
      newStore[key] = stateMock;
    }
  }

  return {
    store: newStore as TStore,
    original: store,
    mocks: mockResult as Partial<{
      [P in keyof TStore]: jest.Mock;
    }>,
  };
};
