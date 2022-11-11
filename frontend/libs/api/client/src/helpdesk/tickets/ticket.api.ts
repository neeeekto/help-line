import { HttpClient } from '@help-line/http';
import { Ticket, TicketAction } from './types';

export const makeTicketApi = (http: HttpClient) => ({
  getById: (ticketId: Ticket['id']) =>
    http.get<Ticket>(`/api/v1/hd/ticket/${ticketId}`).then((x) => x.data),
  getByIdAtDate: (ticketId: Ticket['id'], date: Date) =>
    http
      .get<Ticket>(`/api/v1/hd/ticket/${ticketId}/${date}`)
      .then((x) => x.data),
  execute: (ticketId: Ticket['id'], actions: TicketAction[]) =>
    http
      .post<any[]>(`/api/v1/hd/ticket/${ticketId}`, actions)
      .then((x) => x.data),
  retryMessage: (ticketId: Ticket['id'], messageId: string, userId: string) =>
    http
      .post<void>(
        `/api/v1/hd/ticket/${ticketId}/messages/${userId}/${messageId}/retry`
      )
      .then((x) => x.data),
});
