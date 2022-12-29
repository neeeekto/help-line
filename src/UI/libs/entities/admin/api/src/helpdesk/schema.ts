import { createApiAction, HttpMethod } from '@help-line/modules/http';
import {
  CreateTicketRequest,
  TicketSchedule,
  TicketScheduleStatus,
} from './types';
import { Ticket, TicketId } from '@help-line/entities/client/api';

export const HelpdeskAdminApiSchema = {
  getSchedules: createApiAction<
    TicketSchedule[],
    { statuses: TicketScheduleStatus[] }
  >({
    method: HttpMethod.GET,
    url: '/v1/helpdesk/schedules/',
    params: ({ statuses }) => ({ statuses }),
  }),

  getSchedulesByTicket: createApiAction<
    TicketSchedule[],
    { ticketId: Ticket['id'] }
  >({
    method: HttpMethod.GET,
    url: ({ ticketId }) => `/v1/helpdesk/ticket/${ticketId}/schedules`,
  }),

  reschedule: createApiAction<void, { scheduleId: TicketSchedule['id'] }>({
    method: HttpMethod.POST,
    url: ({ scheduleId }) => `/v1/helpdesk/schedules/${scheduleId}/reschedule/`,
  }),

  delete: createApiAction<void, { scheduleId: TicketSchedule['id'] }>({
    method: HttpMethod.DELETE,
    url: ({ scheduleId }) => `/v1/helpdesk/schedules/${scheduleId}/`,
  }),

  createTicket: createApiAction<Ticket['id'], CreateTicketRequest>({
    method: HttpMethod.POST,
    url: `/v1/helpdesk/ticket`,
    data: (req) => req,
  }),

  recreateTicketView: createApiAction<void, { ticketId: TicketId }>({
    method: HttpMethod.POST,
    url: ({ ticketId }) => `/v1/helpdesk/ticket/${ticketId}/view/recreate/`,
  }),

  getTicketView: createApiAction<Ticket, { ticketId: TicketId }>({
    method: HttpMethod.GET,
    url: ({ ticketId }) => `/v1/helpdesk/ticket/${ticketId}/view/`,
  }),
};
