import React, { StrictMode } from 'react';
import * as ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';

import { setupI18n, RootWrapper } from '@help-line/modules/application';
import { environment } from './environments/environment';

import './styles/global';
import { ThemeProvider } from './styles/theme-provider';
import { AuthGuard } from '@help-line/modules/auth';
import { AppRoutes } from './routes';
import { DiProvider } from './di';

setupI18n();
const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <StrictMode>
    <RootWrapper env={environment}>
      <DiProvider>
        <ThemeProvider>
          <AuthGuard>
            <BrowserRouter>
              <AppRoutes />
            </BrowserRouter>
          </AuthGuard>
        </ThemeProvider>
      </DiProvider>
    </RootWrapper>
  </StrictMode>
);
