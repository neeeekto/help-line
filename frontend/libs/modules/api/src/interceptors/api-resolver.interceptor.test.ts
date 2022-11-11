import { ApiResolverInterceptor } from './api-resolver.interceptor';
import { HttpRequest } from '@help-line/http';

describe('ApiResolverInterceptor', () => {
  const prefix = 'test';
  const serverUrl = 'http://localhost:123';
  const endpoint = 'stub/endpoint';

  it('should create', () => {
    expect(() => new ApiResolverInterceptor(prefix, serverUrl)).not.toThrow();
  });

  describe('should throw error if serverUrl is ', () => {
    xit('empty', () => {
      expect(() => new ApiResolverInterceptor('', '')).toThrow();
    });

    xit('invalid', () => {
      expect(() => new ApiResolverInterceptor('', 'test')).toThrow();
    });
  });

  it('should set correct path', async () => {
    const req: HttpRequest = { url: `/${prefix}/${endpoint}` };

    const interceptor = new ApiResolverInterceptor(prefix, serverUrl);
    await interceptor.intercept(req, {
      handle: jest.fn().mockReturnValue(Promise.resolve()),
    });

    expect(req.url).toBe(`${serverUrl}/${endpoint}/`);
  });

  it('should ignore handling if raw path doesnt contains "/api" string', async () => {
    const req: HttpRequest = { url: endpoint };

    const interceptor = new ApiResolverInterceptor(prefix, serverUrl);
    await interceptor.intercept(req, {
      handle: jest.fn().mockReturnValue(Promise.resolve()),
    });
    expect(req.url).toBe(endpoint);
  });

  it('should delete skip segments', async () => {
    const segment = 'abc';
    const initialUrl = `//${segment}//${segment}//`;
    const req: HttpRequest = { url: `/${prefix}/${initialUrl}` };

    const interceptor = new ApiResolverInterceptor(prefix, serverUrl);
    await interceptor.intercept(req, {
      handle: jest.fn().mockReturnValue(Promise.resolve()),
    });
    expect(req.url).toBe(`${serverUrl}/${segment}/${segment}/`);
  });
});
