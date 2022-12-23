import { useContext, useEffect } from "react";

import { EventsContext } from "./events.contexts";
import { FnMapper } from "./events.type";
import { EventsService, IEventsService } from "@core/events/events.service";

const useEventsService = <TEvents extends FnMapper, TCommands extends FnMapper>(
  hubKey: string,
  events: TEvents,
  commands: TCommands
) => {
  const ctx = useContext(EventsContext);
  let service = ctx.hubs[hubKey];
  if (!service) {
    service = new EventsService<FnMapper, FnMapper>(
      hubKey,
      ctx.accessTokenFactory,
      commands,
      events
    );
    ctx.hubs[hubKey] = service;
  }
  return service;
};

export const makeEventServiceHook =
  <TEvents extends FnMapper, TCommands extends FnMapper>(
    hubKey: string,
    events: TEvents,
    commands: TCommands
  ) =>
  (): IEventsService<TEvents, TCommands> => {
    const service = useEventsService(hubKey, events, commands);
    useEffect(() => {
      service.start();
      return () => {
        service.stop();
      };
    }, [service]);

    return service as IEventsService<TEvents, TCommands>;
  };
