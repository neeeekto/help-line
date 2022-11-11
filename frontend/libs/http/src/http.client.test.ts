import { HttpClient, HttpRequestParams } from './http.client';
import {
  HttpHandler,
  HttpInterceptor,
  HttpMethod,
  HttpRequest,
  HttpResponse,
} from './http.types';
import { anything, deepEqual, instance, mock, verify, when } from 'ts-mockito';

describe('HttpClient', () => {
  let backendMock: HttpHandler;
  let httpClient!: HttpClient;

  beforeEach(() => {
    backendMock = mock<HttpHandler>();
    httpClient = new HttpClient(instance(backendMock));
    when(backendMock.handle(anything())).thenResolve({} as HttpResponse);
  });

  it('should create', () => {
    expect(() => new HttpClient(instance(backendMock))).not.toThrow();
  });

  it('throw error if backend is empty', () => {
    const makeClient = () => new HttpClient(undefined!);
    expect(makeClient).toThrow();
  });

  describe('call backend with correct params', () => {
    const testUrl = '';
    const testData = '';
    const testParams: HttpRequestParams = {
      headers: {
        test: 1,
      },
      params: {
        test: 1,
      },
      withCredentials: true,
      responseType: 'text',
      xsrfCookieName: '',
      xsrfHeaderName: '',
      onUploadProgress: () => {
        return;
      },
      onDownloadProgress: () => {
        return;
      },
    };

    it('for get', () => {
      httpClient.get(testUrl, testParams);

      verify(
        backendMock.handle(
          deepEqual({
            url: testUrl,
            method: HttpMethod.GET,
            ...testParams,
          })
        )
      ).once();
    });

    it('for post', () => {
      httpClient.post(testUrl, testData, testParams);

      verify(
        backendMock.handle(
          deepEqual({
            url: testUrl,
            method: HttpMethod.POST,
            data: testData,
            ...testParams,
          })
        )
      ).once();
    });

    it('for delete', () => {
      httpClient.delete(testUrl, testParams);

      verify(
        backendMock.handle(
          deepEqual({
            url: testUrl,
            method: HttpMethod.DELETE,
            ...testParams,
          })
        )
      ).once();
    });

    it('for put', () => {
      httpClient.put(testUrl, testData, testParams);

      verify(
        backendMock.handle(
          deepEqual({
            url: testUrl,
            method: HttpMethod.PUT,
            data: testData,
            ...testParams,
          })
        )
      ).once();
    });

    it('for patch', () => {
      httpClient.patch(testUrl, testData, testParams);

      verify(
        backendMock.handle(
          deepEqual({
            url: testUrl,
            method: HttpMethod.PATCH,
            data: testData,
            ...testParams,
          })
        )
      ).once();
    });
  });

  describe('interceptors', () => {
    let interceptor: HttpInterceptor;

    beforeEach(() => {
      interceptor = mock<HttpInterceptor>();
      when(interceptor.intercept(anything(), anything())).thenResolve(
        {} as HttpResponse
      );
    });

    it('use interceptor', () => {
      httpClient = new HttpClient(instance(backendMock), [
        instance(interceptor),
      ]);
      httpClient.get('');

      verify(interceptor.intercept(anything(), anything())).once();
    });

    it('can add new interceptor', () => {
      const withInterceptor = httpClient.withInterceptors(
        instance(interceptor)
      );
      withInterceptor.get('');

      verify(interceptor.intercept(anything(), anything())).once();
    });

    it('can delete interceptor', () => {
      class TestInterceptor extends HttpInterceptor {
        intercept(req: HttpRequest, next: HttpHandler): Promise<HttpResponse> {
          return next.handle(req);
        }
      }
      const interceptor = new TestInterceptor();
      const spy = jest.spyOn(interceptor, 'intercept');

      httpClient = new HttpClient(instance(backendMock), [interceptor]);
      const withInterceptor = httpClient.withoutInterceptors(TestInterceptor);
      withInterceptor.get('');

      expect(spy).not.toBeCalled();
    });

    it('deleting interceptor dont affect on parent client', () => {
      class TestInterceptor extends HttpInterceptor {
        intercept(req: HttpRequest, next: HttpHandler): Promise<HttpResponse> {
          return next.handle(req);
        }
      }
      const interceptor = new TestInterceptor();
      const spy = jest.spyOn(interceptor, 'intercept');

      httpClient = new HttpClient(instance(backendMock), [interceptor]);
      const withInterceptor = httpClient.withoutInterceptors(TestInterceptor);
      withInterceptor.get('');
      httpClient.get('');

      expect(spy).toBeCalledTimes(1);
    });

    it('the order of calls is observed', () => {
      class TestInterceptor extends HttpInterceptor {
        intercept(req: HttpRequest, next: HttpHandler): Promise<HttpResponse> {
          return next.handle(req);
        }
      }

      const interceptor1 = new TestInterceptor();
      const interceptor2 = new TestInterceptor();

      const interceptorFn1 = jest.spyOn(interceptor1, 'intercept');
      const interceptorFn2 = jest.spyOn(interceptor2, 'intercept');

      httpClient = new HttpClient(instance(backendMock), [
        interceptor1,
        interceptor2,
      ]);
      httpClient.get('');

      expect(interceptorFn1).toBeCalled();
      expect(interceptorFn2).toBeCalled();
      // check left to right invocations, [1, 2, 3] == 1 -> 2 -> 3
      expect(interceptorFn1.mock.invocationCallOrder[0]).toBeLessThan(
        interceptorFn2.mock.invocationCallOrder[0]
      );
    });
  });
});
