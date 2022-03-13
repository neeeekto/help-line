import { QueryClient, QueryClientProvider, setLogger } from "react-query";
import React from "react";

setLogger({
  log: console.log,
  warn: console.warn,
  error: () => {},
});

const client = new QueryClient({
  defaultOptions: {
    queries: {
      retry: false,
      cacheTime: Number.POSITIVE_INFINITY,
    },
  },
});

export const QueryClientForTest: React.FC = ({ children }) => (
  <QueryClientProvider client={client}>{children}</QueryClientProvider>
);
