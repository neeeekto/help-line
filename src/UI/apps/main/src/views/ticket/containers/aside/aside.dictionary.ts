import { TicketStatusType } from '@help-line/entities/client/api';

export const TicketStatusTypeViewName: Partial<
  Record<TicketStatusType, string>
> = {
  [TicketStatusType.AwaitingReply]: 'Awaiting Reply',
  [TicketStatusType.ForReject]: 'For Reject',
};
