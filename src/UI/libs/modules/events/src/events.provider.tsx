import React, { PropsWithChildren, useEffect, useRef } from 'react';
import { EventsContextData, EventsContext } from './events.contexts';

interface EventsProviderProps extends PropsWithChildren {
  authTokenResolver: () => string | undefined;
  serverUrl: string;
}

export const EventsProvider: React.FC<EventsProviderProps> = ({
  children,
  authTokenResolver,
  serverUrl,
}) => {
  const context = useRef<EventsContextData>({
    accessTokenFactory: () => '',
    hubs: {},
    serverUrl,
  });
  useEffect(() => {
    context.current.accessTokenFactory = () => authTokenResolver() ?? '';
  }, [authTokenResolver]);

  return (
    <EventsContext.Provider value={context.current}>
      {children}
    </EventsContext.Provider>
  );
};
