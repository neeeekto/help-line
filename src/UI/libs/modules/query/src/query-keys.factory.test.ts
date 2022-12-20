import { CreateQueryCtx, createQueryKeys } from './query-keys.factory';

describe('query-keys.factory', () => {
  it('should have root field', () => {
    const root = ['test', '1', 2];
    const keys = createQueryKeys(root, () => ({}));
    expect(keys.root).toBe(root);
  });

  it('should use factory', () => {
    const factory = jest.fn(() => ({}));
    createQueryKeys(['test'], factory);
    expect(factory).toBeCalled();
  });

  it('should call factory with correct ctx', () => {
    const factory = jest.fn(() => ({}));
    const root = ['test', 1, 2];
    createQueryKeys(root, factory);
    const [args] = factory.mock.calls;
    const [ctx] = args as any as [CreateQueryCtx];
    expect(ctx.root).toBe(root);
    expect(typeof ctx.makeKey).toBe('function');
  });

  it('should return object contains factory result', () => {
    const initial = {
      test1: 1,
      test2: 2,
      test3: {
        test31: 31,
        test32: {
          test321: 321,
        },
      },
    };
    const keys = createQueryKeys(['test'], () => initial);
    expect(keys).toEqual(expect.objectContaining(initial));
  });

  it('should make path with root', () => {
    const root = ['test', '1', 2];
    const testKey = ['myKey'];
    const keys = createQueryKeys(root, ({ makeKey }) => ({
      t1: makeKey(...testKey),
    }));
    expect(keys.t1).toEqual(expect.arrayContaining([...root, ...testKey]));
  });
});
