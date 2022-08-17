import { TicketStatusType } from "@entities/helpdesk/tickets";

export const TicketStatusTypeViewName: Partial<
  Record<TicketStatusType, string>
> = {
  [TicketStatusType.AwaitingReply]: "Awaiting Reply",
  [TicketStatusType.ForReject]: "For Reject",
};
