import { PropsWithChildren } from 'react';
import { DiContainer } from '@help-line/modules/di';
import { setupAppDI } from './di';
import { TestQueryProvider } from '@help-line/dev/unit-tests';

export const UnitTestWrapper = ({ children }: PropsWithChildren) => {
  return (
    <DiContainer container={setupAppDI({ apiUrl: '' } as any)}>
      <TestQueryProvider>{children}</TestQueryProvider>
    </DiContainer>
  );
};
