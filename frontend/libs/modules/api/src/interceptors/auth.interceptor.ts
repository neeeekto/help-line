import {
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@help-line/http';

import set from 'lodash/set';

export interface AuthToken {
  value: string;
  type: string;
}

export interface AuthFacade {
  getToken():
    | Promise<AuthToken | undefined | null>
    | AuthToken
    | undefined
    | null;
  logout(): Promise<void>;
}

export class AuthInterceptor extends HttpInterceptor {
  private readonly authFacade: AuthFacade;
  constructor(authFacade: AuthFacade) {
    super();
    this.authFacade = authFacade;
    if (!authFacade) {
      throw new Error('Provide auth facade');
    }
  }

  async intercept(req: HttpRequest, next: HttpHandler): Promise<HttpResponse> {
    const token = await this.authFacade.getToken();
    if (token) {
      set(req, ['headers', 'Authorization'], `${token.type} ${token.value}`);
    }
    try {
      return await next.handle(req);
    } catch (e) {
      const err = e as HttpResponse;
      if (err?.status === 401) {
        await this.authFacade.logout();
      }

      throw e;
    }
  }
}
