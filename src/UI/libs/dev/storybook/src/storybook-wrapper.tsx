import React, { PropsWithChildren } from 'react';
import {
  DefaultHttpProvider,
  QueryProvider,
} from '@help-line/modules/application';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { MemoryRouter } from 'react-router-dom';
import { InitialEntry } from '@remix-run/router';

export const StorybookWrapper = ({
  children,
  initialRoutes,
}: PropsWithChildren<{ initialRoutes?: InitialEntry[] }>) => {
  return (
    <QueryProvider>
      <DefaultHttpProvider config={{ serverUrl: '', apiPrefix: '' }}>
        <MemoryRouter initialEntries={initialRoutes}>{children}</MemoryRouter>
      </DefaultHttpProvider>
      <ReactQueryDevtools position={'bottom-right'} />
    </QueryProvider>
  );
};
