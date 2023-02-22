import { makeQueryAndMutationForRudApi } from '../utils';
import {
  Project,
  ReopenCondition,
  ReopenConditionsClientApi,
} from '@help-line/entities/client/api';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useInjection } from 'inversify-react';

export const {
  useReopenConditionListQuery,
  useSaveReopenConditionMutation,
  useDeleteReopenConditionMutation,
  clientReopenConditionQueryKeys,
} = makeQueryAndMutationForRudApi(
  'ReopenCondition',
  ['reopen-conditions'],
  ReopenConditionsClientApi,
  (args) => [args.projectId]
);

export const useSwitchReopenConditionMutation = (
  projectId: Project['id'],
  toId: ReopenCondition['id']
) => {
  const api = useInjection(ReopenConditionsClientApi);
  const client = useQueryClient();
  return useMutation(
    [...clientReopenConditionQueryKeys.root, 'switch', toId],
    (fromId: ReopenCondition['id']) => api.switch({ fromId, toId }),
    {
      onSuccess: () =>
        client.invalidateQueries(
          clientReopenConditionQueryKeys.list({ projectId })
        ),
    }
  );
};

export const useToggleReopenConditionMutation = (
  projectId: Project['id'],
  reopenConditionId: ReopenCondition['id']
) => {
  const api = useInjection(ReopenConditionsClientApi);
  const client = useQueryClient();
  return useMutation(
    [...clientReopenConditionQueryKeys.root, 'toggle', reopenConditionId],
    () => api.toggle({ reopenConditionId }),
    {
      onSuccess: () =>
        client.invalidateQueries(
          clientReopenConditionQueryKeys.list({ projectId })
        ),
    }
  );
};
