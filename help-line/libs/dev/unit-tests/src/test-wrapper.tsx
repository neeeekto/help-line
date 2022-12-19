import React, { PropsWithChildren } from 'react';
import {
  DefaultHttpProvider,
  QueryProvider,
} from '@help-line/modules/application';

export const UnitTestWrapper: React.FC<PropsWithChildren> = ({ children }) => {
  return (
    <QueryProvider>
      <DefaultHttpProvider config={{ serverUrl: '', apiPrefix: '' }}>
        {children}
      </DefaultHttpProvider>
    </QueryProvider>
  );
};
