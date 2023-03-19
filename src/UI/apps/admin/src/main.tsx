import React, { StrictMode } from 'react';
import * as ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';

import {
  setupI18n,
  RootWrapper,
  registryHttpInDi,
} from '@help-line/modules/application';

import './styles/global';
import { ThemeProvider } from './styles/theme-provider';
import { AuthGuard, registryAuthInDI } from '@help-line/modules/auth';
import { AppRoutes } from './routes';
import { MigrationsProvider } from './views/migrations';
import { LayoutRoot } from './layout';
import { setupAppDI } from './di';
import { environment } from './environments/environment';

setupI18n();
const diContainer = setupAppDI(environment);
const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <StrictMode>
    <RootWrapper container={diContainer}>
      <ThemeProvider>
        <AuthGuard>
          <BrowserRouter>
            <MigrationsProvider>
              <LayoutRoot>
                <AppRoutes />
              </LayoutRoot>
            </MigrationsProvider>
          </BrowserRouter>
        </AuthGuard>
      </ThemeProvider>
    </RootWrapper>
  </StrictMode>
);
