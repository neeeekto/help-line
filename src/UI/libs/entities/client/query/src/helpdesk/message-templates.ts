import { makeQueryAndMutationForCrudApi } from '../utils';
import {
  MessageTemplate,
  MessageTemplateClientApi,
} from '@help-line/entities/client/api';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useInjection } from 'inversify-react';

export const {
  useDeleteMessageTemplateMutation,
  useMessageTemplateListQuery,
  useUpdateMessageTemplateMutation,
  clientMessageTemplateQueryKeys,
  useCreateMessageTemplateMutation,
} = makeQueryAndMutationForCrudApi(
  'MessageTemplate',
  ['message-templates'],
  MessageTemplateClientApi,
  (args) => [args.projectId]
);

export const useChangeOrderMessageTemplateMutation = (
  templateId: MessageTemplate['id'],
  projectId: string
) => {
  const api = useInjection(MessageTemplateClientApi);
  const client = useQueryClient();

  return useMutation(
    [...clientMessageTemplateQueryKeys.root, 'chang-order'],
    (order: number) => api.changeOrder({ order, templateId, projectId }),
    {
      onSuccess: () =>
        client.invalidateQueries(
          clientMessageTemplateQueryKeys.list({ projectId })
        ),
    }
  );
};
