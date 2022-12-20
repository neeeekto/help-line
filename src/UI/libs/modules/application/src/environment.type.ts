export interface IEnvironment {
  production: boolean;
  serverUrl: string;
  apiPrefix: string;
  oauth: {
    authority: string;
    client_id: string;
    redirect_uri: string;
    post_logout_redirect_uri: string;
    response_type: string;
    scope: string;
    silent_redirect_uri: string;
  };
}
