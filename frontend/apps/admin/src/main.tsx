import { StrictMode } from 'react';
import * as ReactDOM from 'react-dom/client';

import App from './app/app';
import { AuthGuard, AuthProvider } from '@help-line/modules/auth';
import { environment } from './environments/environment';
import {
  DefaultEventsProvider,
  DefaultHttpProvider,
  QueryProvider,
} from '@help-line/kernel';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <StrictMode>
    <QueryProvider>
      <AuthProvider settings={environment.oauth}>
        <DefaultHttpProvider env={environment}>
          <App />
        </DefaultHttpProvider>
      </AuthProvider>
    </QueryProvider>
  </StrictMode>
);
