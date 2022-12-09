import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { makeHeaderWithProject, ProjectApiRequest } from '../../schema.share';
import {
  EmailRendererResult,
  EmailFeedbackPreviewRequest,
  EmailMessagePreviewRequest,
} from './types';
import { Ticket } from '../tickets';

export const PreviewClientApiSchema = {
  getFeedback: createApiAction<
    EmailRendererResult,
    EmailFeedbackPreviewRequest & ProjectApiRequest
  >({
    method: HttpMethod.GET,
    url: ({ feedbackId, ticketId }) =>
      `/v1/hd/preview/${ticketId}/feedback/${feedbackId}/`,
    header: makeHeaderWithProject,
  }),

  getMessage: createApiAction<
    EmailRendererResult,
    EmailMessagePreviewRequest & ProjectApiRequest
  >({
    method: HttpMethod.GET,
    url: ({ ticketId }) => `/v1/hd/preview/${ticketId}/message/`,
    header: makeHeaderWithProject,
  }),
};
