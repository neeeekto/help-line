import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import React, { PropsWithChildren, useMemo } from 'react';
import { QueryClientConfig } from '@tanstack/query-core';

interface QueryProviderProps extends PropsWithChildren {
  config?: QueryClientConfig;
}

export const QueryProvider: React.FC<QueryProviderProps> = ({
  children,
  config,
}) => {
  const queryClient = useMemo(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            staleTime: 5 * 20 * 1000,
          },
        },
        ...(config || {}),
      }),
    [config]
  );

  return (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  );
};
