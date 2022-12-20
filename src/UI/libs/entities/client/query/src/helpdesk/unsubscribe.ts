import { useApi } from '@help-line/modules/api';
import {
  Operator,
  Project,
  Unsubscribe,
  UnsubscribeClientApi,
} from '@help-line/entities/client/api';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { ROOT_QUERY_KEY, T_20_MIN } from '../constants';
import { createQueryKeys } from '@help-line/modules/query';

export const clientUnsubscribesQueryKeys = createQueryKeys(
  [ROOT_QUERY_KEY, 'unsubscribe'],
  ({ makeKey }) => ({
    list: (projectId: Project['id']) => makeKey(projectId),
  })
);

export const useUnsubscribesQueries = (projectId: Project['id']) => {
  const api = useApi(UnsubscribeClientApi);
  return useQuery(clientUnsubscribesQueryKeys.list(projectId), () =>
    api.get({ projectId })
  );
};

export const useDeleteUnsubscribeMutation = (
  projectId: Project['id'],
  unsubscribeId: Unsubscribe['id']
) => {
  const api = useApi(UnsubscribeClientApi);
  const client = useQueryClient();

  return useMutation(
    [...clientUnsubscribesQueryKeys.root, 'delete'],
    () => api.delete({ unsubscribeId }),
    {
      onSuccess: () =>
        client.invalidateQueries(clientUnsubscribesQueryKeys.list(projectId)),
    }
  );
};
