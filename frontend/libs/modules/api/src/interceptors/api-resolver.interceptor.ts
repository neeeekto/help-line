import {
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@help-line/http';

export class ApiResolverInterceptor extends HttpInterceptor {
  private readonly apiPrefix: string;
  private readonly targetServerUrl: string;

  constructor(
    apiPrefix: string,
    serverUrl: string,
    private readonly withCredentials = true
  ) {
    super();
    this.apiPrefix = apiPrefix;
    this.targetServerUrl = serverUrl;
    // this.throwIfUrlIsInvalid(serverUrl);
  }

  async intercept(req: HttpRequest, next: HttpHandler): Promise<HttpResponse> {
    if (req?.url?.includes(`/${this.apiPrefix}/`)) {
      const url = new URL(req.url, window.location.origin);
      const path = url.pathname.replace(`/${this.apiPrefix}/`, '/');
      const segments = path.split('/').filter(Boolean);
      const resultPath = [this.targetServerUrl, ...segments].filter(Boolean);
      req.url = `${resultPath.join('/')}/${url.search}${url.hash}`;
      req.withCredentials = this.withCredentials;
    }
    return next.handle(req);
  }
}
