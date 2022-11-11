import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import React, { PropsWithChildren, useMemo } from 'react';
import { QueryClientConfig } from '@tanstack/query-core';

interface QueryProviderProps extends PropsWithChildren {
  config?: QueryClientConfig;
}

export const TestQueryProvider: React.FC<QueryProviderProps> = ({
  children,
  config,
}) => {
  const queryClient = useMemo(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            retry: false,
          },
        },
        logger: {
          log: console.log,
          warn: console.warn,
          error: () => {},
        },
      }),
    [config]
  );

  return (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  );
};
