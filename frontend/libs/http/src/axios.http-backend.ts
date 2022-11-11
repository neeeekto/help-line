import { HttpHandler, HttpRequest, HttpResponse } from './http.types';
import axios, { AxiosError, AxiosInstance, AxiosResponse } from 'axios';
import { stringify } from 'qs';
import { HttpError } from './http.error';

export class AxiosHttpBackend implements HttpHandler {
  private readonly axios: AxiosInstance;
  constructor(serverUrl: string) {
    this.axios = axios.create({
      baseURL: serverUrl,
      withCredentials: false,
      paramsSerializer: (params) =>
        stringify(params, { indices: false, skipNulls: true }),
    });
  }

  async handle(req: HttpRequest): Promise<HttpResponse> {
    try {
      const res = await this.axios.request({
        url: req.url,
        method: req.method,
        baseURL: req.baseURL,
        headers: req.headers,
        params: req.params,
        data: req.data,
        withCredentials: req.withCredentials,
        responseType: req.responseType,
        xsrfCookieName: req.xsrfCookieName,
        xsrfHeaderName: req.xsrfHeaderName,
        onUploadProgress: req.onUploadProgress,
        onDownloadProgress: req.onDownloadProgress,
      });
      return this.toHttpResponse(req, res);
    } catch (e) {
      throw new HttpError(this.toHttpResponse(req, (e as AxiosError).response));
    }
  }

  private toHttpResponse(
    req: HttpRequest,
    response?: AxiosResponse
  ): HttpResponse {
    return {
      data: response?.data,
      headers: response?.headers,
      status: response?.status,
      statusText: response?.statusText,
      config: req,
    } as HttpResponse;
  }
}
