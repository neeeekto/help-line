import { PropsWithChildren, useMemo } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

export const TestQueryProvider = ({ children }: PropsWithChildren) => {
  const qClient = useMemo(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            retry: false,
            staleTime: Number.POSITIVE_INFINITY,
          },
        },
        logger: {
          log: console.log,
          warn: console.warn,
          error: console.error,
        },
      }),
    []
  );
  return <QueryClientProvider client={qClient}>{children}</QueryClientProvider>;
};
