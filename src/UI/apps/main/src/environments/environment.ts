// This file can be replaced during build by using the `fileReplacements` array.
// When building for production, this file is replaced with `environment.prod.ts`.

import { IEnvironment } from '@help-line/modules/application';

export const environment: IEnvironment = {
  apiPrefix: (import.meta as any).env['VITE_API_PREFIX'] || 'api',
  production: (import.meta as any).env['NODE_ENV'] === 'production',
  apiUrl: (import.meta as any).env['VITE_API_SERVER'] || '',
  oauth: {
    authority: (import.meta as any).env['VITE_OAUTH_AUTHORITY']!,
    client_id: (import.meta as any).env['VITE_OAUTH_CLIENT_ID']!,
    redirect_uri: (import.meta as any).env['VITE_OAUTH_REDIRECT_URI']!,
    post_logout_redirect_uri: (import.meta as any).env[
      'VITE_OAUTH_POST_LOGOUT_REDIRECT_URI'
    ]!,
    response_type: (import.meta as any).env['VITE_OAUTH_RESPONSE_TYPE']!,
    scope: (import.meta as any).env['VITE_OAUTH_SCOPE']!,
    silent_redirect_uri: (import.meta as any).env[
      'VITE_OAUTH_SILENT_REDIRECT_URI'
    ]!,
  },
};

console.log(environment);
