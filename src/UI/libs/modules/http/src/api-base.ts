import { HttpClient } from './http.client';

export abstract class ApiBase {
  constructor(protected readonly http: HttpClient) {}
}
