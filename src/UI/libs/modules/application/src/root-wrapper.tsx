import React, { PropsWithChildren } from 'react';
import { QueryProvider } from './query-provider';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { DiContainer } from '@help-line/modules/di';
import { Container } from 'inversify';

export const RootWrapper: React.FC<
  PropsWithChildren<{ container: Container }>
> = ({ children, container }) => {
  return (
    <DiContainer container={container}>
      <QueryProvider>
        {children}
        <ReactQueryDevtools position={'bottom-right'} />
      </QueryProvider>
    </DiContainer>
  );
};
