import { createContext } from "react";
import { EventsService } from "@core/events/events.service";

export interface EventsContextData {
  accessTokenFactory(): string;
  hubs: Record<string, EventsService>;
}

export const EventsContext = createContext<EventsContextData>(null!);
