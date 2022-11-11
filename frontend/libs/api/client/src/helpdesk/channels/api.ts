import { httpClient, HttpClient } from "@core/http";
import { EmailRendererResult } from "./types";
import { Ticket } from "@entities/helpdesk/tickets";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeChannelApi = (http: HttpClient) => ({
  getFeedbackPreview: (ticketId: Ticket["id"], feedbackId: string) =>
    http
      .get<EmailRendererResult>(
        `/api/v1/hd/email/previews/${ticketId}/feedback/${feedbackId}`
      )
      .then((x) => x.data),
  getMessagesPreview: (ticketId: Ticket["id"]) =>
    http
      .get<EmailRendererResult>(
        `/api/v1/hd/email/previews/${ticketId}/messages`
      )
      .then((x) => x.data),
});

export const channelApi = makeChannelApi(httpClient);
