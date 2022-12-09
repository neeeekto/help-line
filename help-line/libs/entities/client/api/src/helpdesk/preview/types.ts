import { Ticket } from '../tickets';

export interface EmailMessagePreviewRequest {
  ticketId: Ticket['id'];
}

export interface EmailFeedbackPreviewRequest
  extends EmailMessagePreviewRequest {
  feedbackId: string;
}

export enum Channels {
  Email = 'email',
  Chat = 'chat',
}

export interface EmailRendererResult {
  body: string;
  subject: string;
}
