import { createContext } from 'react';
import { EventsService } from '@help-line/modules/events';

export interface EventsContextData {
  accessTokenFactory(): string;
  hubs: Record<string, EventsService>;
  serverUrl: string;
}

export const EventsContext = createContext<EventsContextData>(null!);
