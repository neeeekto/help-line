import React, { useEffect, useRef } from "react";
import { EventsContextData, EventsContext } from "./events.contexts";
import { useAuthStore$ } from "@core/auth";

export const EventsProvider: React.FC = ({ children }) => {
  const auth = useAuthStore$();
  const context = useRef<EventsContextData>({
    accessTokenFactory: () => "",
    hubs: {},
  });
  useEffect(() => {
    context.current.accessTokenFactory = () =>
      auth.state?.user?.access_token ?? "";
  }, [auth]);

  return (
    <EventsContext.Provider value={context.current}>
      {children}
    </EventsContext.Provider>
  );
};
