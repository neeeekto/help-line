import React, { PropsWithChildren } from 'react';
import { IEnvironment } from './environment.type';
import { AuthProvider } from '@help-line/modules/auth';
import { DefaultEventsProvider } from './default-events-provider';
import { QueryProvider } from './query-provider';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

export const RootWrapper: React.FC<
  PropsWithChildren<{ env: IEnvironment }>
> = ({ env, children }) => {
  return (
    <QueryProvider>
      <AuthProvider settings={env.oauth}>
        <DefaultEventsProvider env={env}>{children}</DefaultEventsProvider>
      </AuthProvider>
      {!env.production && <ReactQueryDevtools position={'bottom-right'} />}
    </QueryProvider>
  );
};
