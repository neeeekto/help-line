import { EventsService, IEventsServiceConfig } from '@help-line/modules/events';
import { Ticket } from './types';
import { GUID } from '@help-line/entities/share';
import { Operator } from '../operators';
import { Project } from '../projects';

export class TicketsEventsService extends EventsService {
  constructor(cnfg: IEventsServiceConfig) {
    super(
      'tickets',
      {
        OnUpdated: (ticketId: Ticket['id'], newEventsIds: GUID[]) => ({
          ticketId,
          newEventsIds,
        }),
        OnCreated: (ticketId: Ticket['id']) => ({ ticketId }),
        OnOpen: (ticketId: Ticket['id'], operatorId: Operator['id']) => ({
          ticketId,
          operatorId,
        }),
        OnClose: (ticketId: Ticket['id'], operatorId: Operator['id']) => ({
          ticketId,
          operatorId,
        }),
      },
      {
        Subscribe: (projectId: Project['id']) => [projectId],
        Unsubscribe: (projectId: Project['id']) => [projectId],
      },
      cnfg
    );
  }
}

export class TicketEventsService extends EventsService {
  constructor(cnfg: IEventsServiceConfig) {
    super(
      'ticket',
      {
        OnUpdated: (newEventsIds: GUID[]) => ({
          newEventsIds,
        }),
        OnOpen: (operatorId: Operator['id']) => ({
          operatorId,
        }),
        OnClose: (operatorId: Operator['id']) => ({
          operatorId,
        }),
      },
      {
        Subscribe: (ticketId: Ticket['id']) => [ticketId],
        Unsubscribe: (ticketId: Ticket['id']) => [ticketId],
        Open: (
          projectId: Project['id'],
          ticketId: Ticket['id'],
          operatorId: Operator['id']
        ) => [projectId, ticketId, operatorId],
        Close: (
          projectId: Project['id'],
          ticketId: Ticket['id'],
          operatorId: Operator['id']
        ) => [projectId, ticketId, operatorId],
      },
      cnfg
    );
  }
}
