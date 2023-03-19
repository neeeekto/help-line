import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { EmitFns, FnArgsToArgsType, FnMapper } from './events.type';

export interface IEventsService<
  TEvents extends FnMapper = FnMapper,
  TCommands extends FnMapper = FnMapper
> {
  readonly commands: FnArgsToArgsType<TCommands, Promise<any>>;
  add(handler: Partial<EmitFns<TEvents>>): () => void;
  delete(handler: Partial<EmitFns<TEvents>>): void;
}

export interface IEventsServiceConfig {
  serverUrl: string;
  accessTokenFactory: () => string;
}

export class EventsService<
  TEvents extends FnMapper = FnMapper,
  TCommands extends FnMapper = FnMapper
> implements IEventsService<TEvents, TCommands>
{
  readonly commands: FnArgsToArgsType<TCommands, Promise<any>>;
  private readonly connection: HubConnection;

  private handlers: Array<Partial<EmitFns<TEvents>>> = [];
  private init: Promise<any>;

  constructor(
    hubKey: string,
    commands: TCommands,
    events: TEvents,
    config: IEventsServiceConfig
  ) {
    this.connection = new HubConnectionBuilder()
      .withUrl(`${config.serverUrl}/hubs/${hubKey}`, {
        accessTokenFactory: config.accessTokenFactory,
      })
      .withAutomaticReconnect()
      .build();
    this.init = Promise.resolve();

    this.commands = this.makeCommands(commands);
    this.initEvents(events);
  }

  private makeCommands(commands: TCommands) {
    return Object.keys(commands).reduce((res: any, key: string) => {
      res[key] = async (...args: any[]) => {
        try {
          await this.init;
          await this.connection.invoke(key, ...commands[key](...args));
        } catch (e) {
          console.error(e);
        }
      };

      return res;
    }, {});
  }

  private initEvents(events: TEvents) {
    return Object.keys(events).forEach((key) => {
      this.connection.on(key, async (...args) => {
        const handlers = this.getEventHandler(key);
        if (handlers.length) {
          const mapper = events[key];
          const data = mapper(...args);
          for (const handler of handlers) {
            await handler(data);
          }
        }
      });
    });
  }

  add(handler: Partial<EmitFns<TEvents>>) {
    this.handlers.push(handler);
    return () => this.delete(handler);
  }

  delete(handler: Partial<EmitFns<TEvents>>) {
    this.handlers = this.handlers.filter((x) => x !== handler);
  }

  start() {
    if (this.connection.state === HubConnectionState.Disconnected) {
      this.init = this.connection.start();
    }
  }

  stop() {
    if (this.connection.state === HubConnectionState.Connected) {
      this.init = this.connection.stop();
    }
  }

  private getEventHandler(key: string) {
    return this.handlers.map((h) => h[key]!).filter((x) => !!x);
  }
}
