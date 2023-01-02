import { TicketEventBase } from '@help-line/entities/client/api';

export interface EventViewProp<TEvent extends TicketEventBase> {
  event: TEvent;
}
