import { HttpRequestParams } from './http.client';

export enum HttpMethod {
  GET = 'GET',
  DELETE = 'DELETE',
  POST = 'POST',
  PUT = 'PUT',
  PATCH = 'PATCH',
}
export type ResponseType =
  | 'arraybuffer'
  | 'blob'
  | 'document'
  | 'json'
  | 'text'
  | 'stream';

export interface HttpRequest<TData = any> {
  url?: string;
  method?: HttpMethod;
  baseURL?: string;
  headers?: Record<string, any>;
  params?: Record<string, any>;
  data?: TData;
  withCredentials?: boolean;
  responseType?: ResponseType;
  xsrfCookieName?: string;
  xsrfHeaderName?: string;
  onUploadProgress?: (evt: any) => void;
  onDownloadProgress?: (evt: any) => void;
}

export interface HttpResponseEvent {
  config: HttpRequest;
}

export interface HttpResponse<TData = unknown> extends HttpResponseEvent {
  data: TData;
  status?: number;
  statusText?: string;
  headers?: Record<string, any>;
}

export interface HttpHandler {
  handle(req: HttpRequest): Promise<HttpResponse>;
}

export type HttpInterceptRequest = Required<HttpRequest>;
export abstract class HttpInterceptor {
  abstract intercept(
    req: HttpInterceptRequest,
    next: HttpHandler
  ): Promise<HttpResponse>;
}

export interface IHttpClient {
  get<TResult>(
    url: string,
    params?: HttpRequestParams
  ): Promise<HttpResponse<TResult>>;
  post<TResult>(
    url: string,
    data?: unknown,
    params?: HttpRequestParams
  ): Promise<HttpResponse<TResult>>;
  patch<TResult>(
    url: string,
    data: unknown,
    params?: HttpRequestParams
  ): Promise<HttpResponse<TResult>>;

  put<TResult>(
    url: string,
    data: unknown,
    params?: HttpRequestParams
  ): Promise<HttpResponse<TResult>>;
  delete(url: string, params?: HttpRequestParams): Promise<HttpResponse<void>>;
}
