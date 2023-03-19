import { PropsWithChildren, useMemo } from 'react';
import { Provider } from 'inversify-react';
import { Container } from 'inversify';

export const TestDiProvider = ({ children }: PropsWithChildren) => {
  const diContainer = useMemo(
    () => new Container({ defaultScope: 'Singleton' }),
    []
  );
  return <Provider container={diContainer}>{children}</Provider>;
};
