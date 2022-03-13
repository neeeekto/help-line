import { createContext, useContext } from "react";
import { Ticket, TicketAction } from "@entities/helpdesk/tickets";

export interface TicketRootContextValue {
  ticket: Ticket;
  readonly?: boolean;
  onExecute: (action: TicketAction) => Promise<any>;
}
export const TicketRootContext = createContext<TicketRootContextValue>(
  {} as any
);

export const useTicketContext = () => useContext(TicketRootContext);
