import { makeQueryAndMutationForRudApi } from '../utils';
import {
  Project,
  ReopenConditionsClientApi,
  TagDescriptionsClientApi,
  TagsClientApi,
} from '@help-line/entities/client/api';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useInjection } from 'inversify-react';

export const {
  useSaveTagDescriptionMutation,
  useTagDescriptionListQuery,
  useDeleteTagDescriptionMutation,
  clientTagDescriptionQueryKeys,
} = makeQueryAndMutationForRudApi(
  'TagDescription',
  ['tags-descriptions'],
  TagDescriptionsClientApi,
  (args) => [args.projectId]
);

export const {
  useSaveTagMutation,
  useTagListQuery,
  useDeleteTagMutation,
  clientTagQueryKeys,
} = makeQueryAndMutationForRudApi('Tag', ['tags'], TagsClientApi, (args) => [
  args.projectId,
]);

export const useSaveManyTagsMutation = ({
  projectId,
}: {
  projectId: Project['id'];
}) => {
  const api = useInjection(TagsClientApi);
  const client = useQueryClient();
  return useMutation(
    [...clientTagQueryKeys.root, projectId, 'save-many'],
    (params: { tags: string[]; enabled: boolean }) =>
      api.saveMany({ ...params, projectId }),
    {
      onSuccess: () =>
        client.invalidateQueries(clientTagQueryKeys.list({ projectId })),
    }
  );
};
