import React, { PropsWithChildren } from 'react';
import {
  DefaultHttpProvider,
  QueryProvider,
} from '@help-line/modules/application';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

export const StorybookWrapper: React.FC<PropsWithChildren> = ({ children }) => {
  return (
    <QueryProvider>
      <DefaultHttpProvider config={{ serverUrl: '', apiPrefix: '' }}>
        {children}
      </DefaultHttpProvider>
      <ReactQueryDevtools position={'bottom-right'} />
    </QueryProvider>
  );
};
