import {
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@help-line/modules/http';

import set from 'lodash/set';
import { AuthStore } from '@help-line/modules/auth';

export class AuthInterceptor extends HttpInterceptor {
  private readonly authStore: AuthStore;
  constructor(authStore: AuthStore) {
    super();
    this.authStore = authStore;
    if (!authStore) {
      throw new Error('Provide auth store service');
    }
  }

  async intercept(req: HttpRequest, next: HttpHandler): Promise<HttpResponse> {
    if (this.authStore.token) {
      set(
        req,
        ['headers', 'Authorization'],
        `${this.authStore.token.type} ${this.authStore.token.value}`
      );
    }
    try {
      return await next.handle(req);
    } catch (e) {
      const err = e as HttpResponse;
      if (err?.status === 401) {
        await this.authStore.clearSession();
      }

      throw e;
    }
  }
}
