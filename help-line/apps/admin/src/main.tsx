import React, { StrictMode } from 'react';
import * as ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';

import App from './app/app';
import { setupI18n } from '@help-line/modules/application';
import { environment } from './environments/environment';
import { RootWrapper } from '../../../libs/modules/application/src/root-wrapper';

import './styles/global';
import { ThemeProvider } from './styles/theme-provider';
import { AuthGuard } from '@help-line/modules/auth';
import css from './main.module.scss';

setupI18n();
const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <StrictMode>
    <RootWrapper env={environment}>
      <ThemeProvider>
        <AuthGuard>
          <BrowserRouter>
            <App />
          </BrowserRouter>
        </AuthGuard>
      </ThemeProvider>
    </RootWrapper>
  </StrictMode>
);
