import React, { PropsWithChildren } from 'react';
import { QueryProvider } from '@help-line/modules/application';

export const UnitTestWrapper: React.FC<PropsWithChildren> = ({ children }) => {
  return <QueryProvider>{children}</QueryProvider>;
};
