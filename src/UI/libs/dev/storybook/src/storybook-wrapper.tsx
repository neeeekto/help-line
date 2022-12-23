import React, { PropsWithChildren } from 'react';
import {
  DefaultHttpProvider,
  QueryProvider,
} from '@help-line/modules/application';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { MemoryRouter } from 'react-router-dom';

export const StorybookWrapper: React.FC<PropsWithChildren> = ({ children }) => {
  return (
    <QueryProvider>
      <DefaultHttpProvider config={{ serverUrl: '', apiPrefix: '' }}>
        <MemoryRouter>{children}</MemoryRouter>
      </DefaultHttpProvider>
      <ReactQueryDevtools position={'bottom-right'} />
    </QueryProvider>
  );
};
