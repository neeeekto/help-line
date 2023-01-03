import { createContext, useContext } from 'react';
import { Ticket, TicketAction } from '@help-line/entities/client/api';

// We use CTX because this view we will use for other pages: ticket, ticket for interns, qt and other
export interface TicketRootContextValue {
  ticket: Ticket;
  readonly?: boolean;
  onExecute: (action: TicketAction) => Promise<any>;
}
export const TicketRootContext = createContext<TicketRootContextValue>(null!);

export const useTicketContext = () => useContext(TicketRootContext);
