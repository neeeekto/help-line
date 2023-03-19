import { PropsWithChildren, useMemo } from 'react';
import { Container } from 'inversify';
import { Provider } from 'inversify-react';

export const DiContainer = ({
  children,
  container,
}: PropsWithChildren & { container: Container }) => {
  return <Provider container={container}>{children}</Provider>;
};
