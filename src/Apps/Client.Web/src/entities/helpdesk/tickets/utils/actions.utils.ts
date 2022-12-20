import { Ticket, TicketStatusKind, TicketStatusType } from "../types";

export const checkCanReject = (ticket: Ticket) => {
  if (
    ticket.status.kind === TicketStatusKind.Closed ||
    ticket.status.type === TicketStatusType.ForReject ||
    ticket.status.type === TicketStatusType.Resolved
  ) {
    return false;
  }
  if (ticket.status.type === TicketStatusType.New) {
    return true;
  }

  return true;
};
