import { createQueryKeys } from '@help-line/modules/query';
import { ROOT_QUERY_KEY } from '../constants';
import {
  EmailFeedbackPreviewRequest,
  EmailMessagePreviewRequest,
  PreviewClientApi,
  ProjectApiRequest,
} from '@help-line/entities/client/api';
import { useQuery } from '@tanstack/react-query';
import { useInjection } from 'inversify-react';

const clientPreviewQueryKeys = createQueryKeys(
  [ROOT_QUERY_KEY, 'preview'],
  ({ makeKey }) => ({
    feedback: (args: EmailFeedbackPreviewRequest & ProjectApiRequest) =>
      makeKey(args.projectId, 'feedback', args.ticketId, args.feedbackId),
    messages: (args: EmailMessagePreviewRequest & ProjectApiRequest) =>
      makeKey(args.projectId, 'message', args.ticketId),
  })
);

export const useTicketFeedbackPreviewQuery = (
  req: EmailFeedbackPreviewRequest & ProjectApiRequest
) => {
  const api = useInjection(PreviewClientApi);
  return useQuery(clientPreviewQueryKeys.feedback(req), () =>
    api.getFeedback(req)
  );
};

export const useTicketMessagesPreviewQuery = (
  req: EmailMessagePreviewRequest & ProjectApiRequest
) => {
  const api = useInjection(PreviewClientApi);

  return useQuery(clientPreviewQueryKeys.messages(req), () =>
    api.getMessage(req)
  );
};
