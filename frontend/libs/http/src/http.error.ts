import { HttpRequest, HttpResponse } from './http.types';
import cloneDeep from 'lodash/cloneDeep';

export class HttpError extends Error implements HttpResponse {
  readonly config: Readonly<HttpRequest>;
  readonly data: any;
  readonly headers?: Readonly<Record<string, any>>;
  readonly status?: number;
  readonly statusText?: string;

  constructor(response: HttpResponse) {
    super();
    this.config = Object.freeze(cloneDeep(response?.config));
    this.data = response?.data;
    this.headers = Object.freeze(cloneDeep(response?.headers));
    this.status = response?.status;
    this.statusText = response?.statusText;
  }
}
