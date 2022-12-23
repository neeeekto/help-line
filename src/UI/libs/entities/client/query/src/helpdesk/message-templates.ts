import { makeQueryAndMutationForCrudApi } from '../utils';
import {
  MessageTemplate,
  MessageTemplateClientApi,
} from '@help-line/entities/client/api';
import { useApi } from '@help-line/modules/api';
import { useMutation, useQueryClient } from '@tanstack/react-query';

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
  const api = useApi(MessageTemplateClientApi);
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
