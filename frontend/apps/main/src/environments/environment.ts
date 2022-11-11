// This file can be replaced during build by using the `fileReplacements` array.
// When building for production, this file is replaced with `environment.prod.ts`.

import { IEnvironment } from '@help-line/modules/root';

export const environment: IEnvironment = {
  apiPrefix: process.env['REACT_APP_API_PREFIX'] || 'api',
  production: process.env['NODE_ENV'] === 'production',
  serverUrl: process.env['REACT_APP_API_SERVER'] || '',
  oauth: {
    authority: process.env['REACT_APP_OAUTH_AUTHORITY']!,
    client_id: process.env['REACT_APP_OAUTH_CLIENT_ID']!,
    redirect_uri: process.env['REACT_APP_OAUTH_REDIRECT_URI']!,
    post_logout_redirect_uri:
      process.env['REACT_APP_OAUTH_POST_LOGOUT_REDIRECT_URI']!,
    response_type: process.env['REACT_APP_OAUTH_RESPONSE_TYPE']!,
    scope: process.env['REACT_APP_OAUTH_SCOPE']!,
    silent_redirect_uri: process.env['REACT_APP_OAUTH_SILENT_REDIRECT_URI']!,
  },
};
