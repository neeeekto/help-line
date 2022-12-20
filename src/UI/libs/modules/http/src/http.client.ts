import {
  HttpHandler,
  HttpInterceptor,
  HttpInterceptRequest,
  HttpMethod,
  HttpRequest,
  HttpResponse,
  IHttpClient,
} from './http.types';

class HttpInterceptorHandler implements HttpHandler {
  constructor(
    private next: HttpHandler,
    private interceptor: HttpInterceptor
  ) {}

  handle(req: HttpRequest): Promise<HttpResponse> {
    return this.interceptor.intercept(req as HttpInterceptRequest, this.next);
  }
}

class NoopInterceptor extends HttpInterceptor {
  intercept(req: HttpRequest, next: HttpHandler): Promise<HttpResponse> {
    return next.handle(req);
  }
}

// ===============

export type HttpRequestParams = Partial<
  Omit<HttpRequest, 'baseURL' | 'method' | 'url' | 'data'>
>;

export class HttpClient implements IHttpClient {
  private readonly backend!: HttpHandler;
  private interceptors: HttpInterceptor[] = [new NoopInterceptor()];
  private chain!: HttpHandler;

  constructor(backend: HttpHandler, defaultInterceptors?: HttpInterceptor[]) {
    if (!backend) {
      throw new Error('Backend must be defined');
    }
    this.backend = backend;

    if (defaultInterceptors?.length) {
      this.interceptors.unshift(...defaultInterceptors);
    }
    this.chain = this.createChain();
  }

  public get<TResult>(url: string, params?: HttpRequestParams) {
    return this.fetch<TResult>({ method: HttpMethod.GET, url, ...params });
  }

  public post<TResult>(
    url: string,
    data?: unknown,
    params?: HttpRequestParams
  ) {
    return this.fetch<TResult>({
      method: HttpMethod.POST,
      url,
      data,
      ...params,
    });
  }

  public patch<TResult>(
    url: string,
    data: unknown,
    params?: HttpRequestParams
  ) {
    return this.fetch<TResult>({
      method: HttpMethod.PATCH,
      url,
      data,
      ...params,
    });
  }

  public put<TResult>(url: string, data: unknown, params?: HttpRequestParams) {
    return this.fetch<TResult>({
      method: HttpMethod.PUT,
      url,
      data,
      ...params,
    });
  }

  public delete(url: string, params?: HttpRequestParams) {
    return this.fetch<void>({
      method: HttpMethod.DELETE,
      url,
      ...params,
    });
  }

  fetch<TData>(req: HttpRequest): Promise<HttpResponse<TData>> {
    return this.chain.handle(req) as Promise<HttpResponse<TData>>;
  }

  withInterceptors(...interceptors: HttpInterceptor[]): HttpClient {
    return new HttpClient(this.backend, [
      ...interceptors,
      ...this.interceptors.slice(-1),
    ]);
  }

  withoutInterceptors(...interceptors: NewableFunction[]): HttpClient {
    return new HttpClient(
      this.backend,
      this.interceptors.filter(
        (i) =>
          !interceptors.some((interceptorType) => i instanceof interceptorType)
      )
    );
  }

  private createChain() {
    return this.interceptors.reduceRight(
      (next, interceptor) => new HttpInterceptorHandler(next, interceptor),
      this.backend
    );
  }
}
