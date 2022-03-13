import { httpClient, HttpClient } from "@core/http";
import {
  CreateTicketRequest,
  TicketSchedule,
  TicketScheduleStatus,
} from "./types";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeHelpdeskApi = (http: HttpClient) => ({
  getSchedules: (statuses: TicketScheduleStatus[]) =>
    http
      .get<TicketSchedule[]>(`/api/v1/helpdesk/schedules`, {
        params: {
          statuses,
        },
      })
      .then((x) => x.data),
  getSchedulesByTicket: (ticketId: string) =>
    http
      .get<TicketSchedule[]>(`/api/v1/helpdesk/schedules/${ticketId}`)
      .then((x) => x.data),
  reschedule: (scheduleId: TicketSchedule["id"]) =>
    http
      .post(`/api/v1/helpdesk/schedules/${scheduleId}/reschedule`)
      .then((x) => x.data),
  delete: (scheduleId: TicketSchedule["id"]) =>
    http.delete(`/api/v1/helpdesk/schedules/${scheduleId}`).then((x) => x.data),
  createTicket: (data: CreateTicketRequest) =>
    http.post<string>(`/api/v1/helpdesk/ticket`, data).then((x) => x.data),

  recreateTicketView: (ticketId: string) =>
    http
      .post(`/api/v1/helpdesk/ticket/${ticketId}/view/recreate`)
      .then((x) => x.data),
  getTicketView: (ticketId: string) =>
    http
      .get<any>(`/api/v1/helpdesk/ticket/${ticketId}/view`)
      .then((x) => x.data),
});

export const helpdeskApi = makeHelpdeskApi(httpClient);
