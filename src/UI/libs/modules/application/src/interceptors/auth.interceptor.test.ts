import { AuthInterceptor } from './auth.interceptor';
import { mock, instance, anything, when, reset, verify } from 'ts-mockito';
import { HttpHandler, HttpRequest, HttpError } from '@help-line/modules/http';
import { AuthStore } from '@help-line/modules/auth';

describe('AuthInterceptor', () => {
  const token = { type: 'test', value: '123' };
  const authFacadeMock = mock<AuthStore>();
  const interceptor = new AuthInterceptor(instance(authFacadeMock));
  let nextMock: HttpHandler;

  beforeEach(() => {
    reset(authFacadeMock);
    when(authFacadeMock.token).thenReturn(token);
    nextMock = mock<HttpHandler>();
    when(nextMock.handle(anything())).thenResolve({} as any);
  });

  it('should throw error if auth dep is null', async () => {
    expect(() => new AuthInterceptor(null!)).toThrow();
  });

  it('should do nothing if there is no token', async () => {
    when(authFacadeMock.token).thenReturn(null);
    const req: HttpRequest = {};
    await interceptor.intercept(req, instance(nextMock));

    expect(req?.headers?.['Authorization']).toBeFalsy();
  });

  it('should set token', async () => {
    const req: HttpRequest = {};
    await interceptor.intercept(req, instance(nextMock));

    expect(req?.headers?.['Authorization']).toBeTruthy();
  });

  it('should get token from auth facade', async () => {
    const req: HttpRequest = {};
    await interceptor.intercept(req, instance(nextMock));

    verify(authFacadeMock.token).called();
  });

  it('should handle 401 error', () => {
    expect.assertions(1);
    const req: HttpRequest = {};
    when(authFacadeMock.logout()).thenReturn(Promise.resolve());
    when(nextMock.handle(anything())).thenReject(
      new HttpError({ config: req, status: 401, data: null })
    );

    return interceptor.intercept(req, instance(nextMock)).catch((e) => {
      const err = e as HttpError;
      expect(err.status).toBe(401);
      verify(authFacadeMock.clearSession()).once();
    });
  });

  it('should ignore error without status code', () => {
    expect.assertions(1);
    const req: HttpRequest = {};
    when(authFacadeMock.logout()).thenReturn(Promise.resolve());
    when(nextMock.handle(anything())).thenReject(
      new HttpError({ config: req, data: null })
    );

    return interceptor.intercept(req, instance(nextMock)).catch((e) => {
      const err = e as HttpError;
      expect(err.status).toBeUndefined();
      verify(authFacadeMock.logout()).never();
    });
  });

  it('should ignore unknown error ', () => {
    expect.assertions(1);
    const req: HttpRequest = {};
    when(authFacadeMock.logout()).thenReturn(Promise.resolve());
    when(nextMock.handle(anything())).thenReject(new Error());

    return interceptor.intercept(req, instance(nextMock)).catch((e) => {
      const err = e as HttpError;
      expect(err.status).toBeUndefined();
      verify(authFacadeMock.logout()).never();
    });
  });

  test.each([400, 403, 404, 409, 500, 503])(
    'should ignore %d error',
    async (code) => {
      expect.assertions(1);
      const req: HttpRequest = {};
      when(authFacadeMock.logout()).thenReturn(Promise.resolve());
      when(nextMock.handle(anything())).thenReject(
        new HttpError({ config: req, status: code, data: null })
      );

      await interceptor.intercept(req, instance(nextMock)).catch((e) => {
        const err = e as HttpError;
        expect(err.status).toBe(code);
        verify(authFacadeMock.logout()).never();
      });
    }
  );
});
