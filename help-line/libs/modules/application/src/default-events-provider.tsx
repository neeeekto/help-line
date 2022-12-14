import React, { PropsWithChildren, useCallback } from 'react';
import { EventsProvider } from '@help-line/modules/events';
import { useAuthUser } from '@help-line/modules/auth';
import { IEnvironment } from './environment.type';

interface DefaultEventsProviderProps extends PropsWithChildren {
  env: IEnvironment;
}

export const DefaultEventsProvider: React.FC<DefaultEventsProviderProps> = ({
  children,
  env,
}) => {
  const user = useAuthUser();
  const tokenResolver = useCallback(() => user?.access_token, [user]);

  return (
    <EventsProvider authTokenResolver={tokenResolver} serverUrl={env.serverUrl}>
      {children}
    </EventsProvider>
  );
};