import { TicketEventBase } from "@entities/helpdesk/tickets";

export interface EventViewProp<TEvent extends TicketEventBase> {
  event: TEvent;
}
