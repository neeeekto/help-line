import React, { PropsWithChildren } from 'react';
import { TestQueryProvider } from './test-query-provider';
import { TestDiProvider } from './test-di-provider';

export const UnitTestWrapper: React.FC<PropsWithChildren> = ({ children }) => {
  return (
    <TestDiProvider>
      <TestQueryProvider>{children}</TestQueryProvider>
    </TestDiProvider>
  );
};
