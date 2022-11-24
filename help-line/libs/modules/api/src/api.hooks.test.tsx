import { HttpClient } from '@help-line/modules/http';
import { renderHook, act } from '@testing-library/react';
import React, { PropsWithChildren, useContext, useMemo } from 'react';
import { HttpContext } from './http.context';
import {
  useApi,
  useCustomApiFactory,
  useCustomHttpClientForApi,
} from './api.hooks';
import {
  ApiContext,
  ApiFactoryContext,
  ApiHttpFactoryContext,
} from './api.context';

describe('api.hooks', () => {
  const http = new HttpClient({
    handle: jest.fn(),
  });
  const wrapper: React.FC<PropsWithChildren> = ({ children }) => {
    // eslint-disable-next-line react-hooks/rules-of-hooks
    const apiCache = useMemo(() => new Map(), []);
    const apiFactories = useMemo(() => new Map(), []);
    const defaultHttps = useMemo(() => new Map(), []);

    return (
      <HttpContext.Provider value={http}>
        <ApiFactoryContext.Provider value={apiFactories}>
          <ApiHttpFactoryContext.Provider value={defaultHttps}>
            <ApiContext.Provider value={apiCache}>
              {children}
            </ApiContext.Provider>
          </ApiHttpFactoryContext.Provider>
        </ApiFactoryContext.Provider>
      </HttpContext.Provider>
    );
  };

  describe('useApi', () => {
    const apiFactory = jest
      .fn()
      .mockImplementation((http: HttpClient) => ({ get: () => http.get('') }));

    beforeEach(() => {
      apiFactory.mockClear();
    });

    it('should throw error if factory is not function', () => {
      expect(() => renderHook(() => useApi({} as any), { wrapper })).toThrow();
    });

    it('should throw error if http context not defined', () => {
      expect(() => renderHook(() => useApi(apiFactory))).toThrow();
    });

    it('should throw error if apiCache context not defined', () => {
      expect(() =>
        renderHook(() => useApi(apiFactory), {
          wrapper: (({ children }) => (
            <HttpContext.Provider value={http}>{children}</HttpContext.Provider>
          )) as React.FC<PropsWithChildren>,
        })
      ).toThrow();
    });

    it('should create', () => {
      const { result } = renderHook(() => useApi(apiFactory), { wrapper });

      expect(result.current).toBeTruthy();
    });

    it('should call factory for first usage', () => {
      renderHook(() => useApi(apiFactory), { wrapper });
      expect(apiFactory).toBeCalled();
    });

    it('should pass http in argument', () => {
      renderHook(() => useApi(apiFactory), { wrapper });
      expect(apiFactory).toBeCalledWith(http);
    });

    it('should use cache if factory call some times', () => {
      const { rerender } = renderHook(() => useApi(apiFactory), {
        wrapper,
      });
      rerender();

      expect(apiFactory).toBeCalledTimes(1);
    });

    it('should use custom factory', () => {
      const customFactory = jest.fn((http) => apiFactory(http));
      const { rerender } = renderHook(() => useApi(apiFactory, customFactory), {
        wrapper,
      });
      rerender();

      expect(apiFactory).toBeCalledTimes(1);
      expect(customFactory).toBeCalledTimes(1);
      expect(customFactory).toBeCalledWith(http);
    });
  });
  describe('useCustomHttpClientForApi', () => {
    const apiCctor = function () {};
    const factory = jest.fn(() => http);

    beforeEach(() => {
      factory.mockClear();
    });

    it('throw error if http ctx not defined ', () => {
      expect(() =>
        renderHook(() => useCustomHttpClientForApi(apiCctor, () => ({} as any)))
      ).toThrow();
    });
    it('throw error if registry ctx not defined', () => {
      expect(() =>
        renderHook(
          () => useCustomHttpClientForApi(apiCctor, () => ({} as any)),
          {
            wrapper: (({ children }) => (
              <HttpContext.Provider value={http}>
                {children}
              </HttpContext.Provider>
            )) as React.FC<PropsWithChildren>,
          }
        )
      ).toThrow();
    });
    it('use factory', () => {
      renderHook(() => useCustomHttpClientForApi({} as any, factory), {
        wrapper,
      });
      expect(factory).toBeCalledWith(http);
    });
    it('save factory result', () => {
      const { result, rerender } = renderHook(
        () => {
          useCustomHttpClientForApi(apiCctor, factory);
          return useContext(ApiHttpFactoryContext);
        },
        {
          wrapper,
        }
      );
      expect(result.current?.get(apiCctor)).toEqual(http);
    });

    it('rerun factory if http changed', () => {
      const factoryCtxVal = new Map();
      const { result, rerender } = renderHook(
        () => useCustomHttpClientForApi(apiCctor, factory),
        {
          wrapper: ({ children }) => (
            <HttpContext.Provider
              value={
                new HttpClient({
                  handle: jest.fn(),
                })
              }
            >
              <ApiHttpFactoryContext.Provider value={factoryCtxVal}>
                {children}
              </ApiHttpFactoryContext.Provider>
            </HttpContext.Provider>
          ),
        }
      );
      act(() => {
        rerender();
      });
      expect(factory).toBeCalledTimes(2);
    });
    it('rerun factory if ctx(registry) recreated', () => {
      const { rerender } = renderHook(
        () => useCustomHttpClientForApi(apiCctor, factory),
        {
          wrapper: ({ children }) => (
            <HttpContext.Provider value={http}>
              <ApiHttpFactoryContext.Provider value={new Map()}>
                {children}
              </ApiHttpFactoryContext.Provider>
            </HttpContext.Provider>
          ),
        }
      );
      act(() => {
        rerender();
      });
      expect(factory).toBeCalledTimes(2);
    });
  });
  describe('useCustomApiFactory', () => {
    const apiCctor: any = function () {};
    const factory = jest.fn(() => new apiCctor());
    it('throw error if registry ctx not defined', () => {
      expect(() =>
        renderHook(() => useCustomApiFactory(apiCctor, () => ({} as any)))
      ).toThrow();
    });
    it('save factory', () => {
      const { result, rerender } = renderHook(
        () => {
          useCustomApiFactory(apiCctor, factory);
          return useContext(ApiFactoryContext);
        },
        {
          wrapper,
        }
      );
      expect(result.current?.get(apiCctor)).toEqual(factory);
    });
    it('rerun factory if ctx(registry) recreated', () => {
      const { rerender, result } = renderHook(
        () => {
          useCustomApiFactory(apiCctor, factory);
          return useContext(ApiFactoryContext);
        },
        {
          wrapper: ({ children }) => (
            <ApiFactoryContext.Provider value={new Map()}>
              {children}
            </ApiFactoryContext.Provider>
          ),
        }
      );

      act(() => {
        rerender();
      });
      expect(result.current.get(apiCctor)).toEqual(factory);
    });
  });
});
