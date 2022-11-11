import { FnMapper } from './events.type';
import { EventsService, IEventsService } from './events.service';
import { useContext, useEffect } from 'react';
import { EventsContext } from './events.contexts';

const useEventsService = <TEvents extends FnMapper, TCommands extends FnMapper>(
  hubKey: string,
  events: TEvents,
  commands: TCommands
): EventsService<TEvents, TCommands> => {
  const ctx = useContext(EventsContext);
  let service = ctx.hubs[hubKey];
  if (!service) {
    service = new EventsService<TEvents, TCommands>(
      ctx.serverUrl,
      hubKey,
      ctx.accessTokenFactory,
      commands,
      events
    );
    ctx.hubs[hubKey] = service;
  }
  return service as EventsService<TEvents, TCommands>;
};

export const makeUseEventServiceHook =
  <TEvents extends FnMapper, TCommands extends FnMapper>(
    serverUrl: string,
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
