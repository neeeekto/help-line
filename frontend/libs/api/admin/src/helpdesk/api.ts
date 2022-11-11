import { ApiBase } from '@help-line/http';
import {
  CreateTicketRequest,
  TicketSchedule,
  TicketScheduleStatus,
} from './types';
import { Ticket } from '@help-line/api/client';

export class HelpdeskAdminApi extends ApiBase {
  public async getSchedules(statuses: TicketScheduleStatus[]) {
    return this.http
      .get<TicketSchedule[]>(`/v1/helpdesk/schedules/`, {
        params: {
          statuses,
        },
      })
      .then((x) => x.data);
  }
  public async getSchedulesByTicket(ticketId: string) {
    return this.http
      .get<TicketSchedule[]>(`/v1/helpdesk/schedules/${ticketId}/`)
      .then((x) => x.data);
  }
  public async reschedule(scheduleId: TicketSchedule['id']) {
    return this.http
      .post(`/v1/helpdesk/schedules/${scheduleId}/reschedule/`)
      .then((x) => x.data);
  }
  public async delete(scheduleId: TicketSchedule['id']) {
    return this.http
      .delete(`/v1/helpdesk/schedules/${scheduleId}/`)
      .then((x) => x.data);
  }
  public async createTicket(data: CreateTicketRequest) {
    return this.http
      .post<string>(`/v1/helpdesk/ticket/`, data)
      .then((x) => x.data);
  }

  public async recreateTicketView(ticketId: string) {
    return this.http
      .post(`/v1/helpdesk/ticket/${ticketId}/view/recreate/`)
      .then((x) => x.data);
  }
  public async getTicketView(ticketId: string) {
    return this.http
      .get<Ticket>(`/v1/helpdesk/ticket/${ticketId}/view/`)
      .then((x) => x.data);
  }
}
