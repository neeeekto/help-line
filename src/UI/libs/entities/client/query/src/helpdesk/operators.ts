import { makeQueryAndMutationForCrudApi } from '../utils';
import {
  Operator,
  OperatorRole,
  OperatorsClientApi,
  OperatorsRolesClientApi,
  Project,
  Ticket,
} from '@help-line/entities/client/api';
import { useApi } from '@help-line/modules/api';
import { createQueryKeys } from '@help-line/modules/query';
import { ROOT_QUERY_KEY } from '../constants';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';

export const clientOperatorsQueryKeys = createQueryKeys(
  [ROOT_QUERY_KEY, 'operators'],
  ({ makeKey }) => ({
    get: createQueryKeys(makeKey('list'), ({ makeKey }) => ({
      list: (projectId: Project['id']) => makeKey(projectId),
      one: (projectId: Project['id'], operatorId: Operator['id']) =>
        makeKey(projectId, operatorId),
    })),
  })
);

export const useOperatorsQuery = (projectId: Project['id']) => {
  const api = useApi(OperatorsClientApi);
  return useQuery(clientOperatorsQueryKeys.get.list(projectId), () =>
    api.get({ projectId })
  );
};

export const useChangeFavoriteMutation = (
  projectId: Project['id'],
  operatorId: Operator['id']
) => {
  const client = useQueryClient();
  const api = useApi(OperatorsClientApi);
  return useMutation(
    [...clientOperatorsQueryKeys.root, 'favorite', operatorId],
    (data: { ticketId: Ticket['id']; needAdd: boolean }) => {
      return data.needAdd
        ? api.addFavorite({ ticketId: data.ticketId, operatorId })
        : api.removeFavorite({ ticketId: data.ticketId, operatorId });
    },
    {
      onSuccess: (_, params) =>
        client.invalidateQueries(clientOperatorsQueryKeys.get.list(projectId)),
    }
  );
};

export const useSetOperatorRoleMutation = (
  projectId: Project['id'],
  operatorId: Operator['id']
) => {
  const client = useQueryClient();
  const api = useApi(OperatorsClientApi);
  return useMutation(
    [clientOperatorsQueryKeys.root, projectId, operatorId, 'roles'],
    (rolesIds: string[]) => api.setRoles({ projectId, operatorId, rolesIds }),
    {
      onSuccess: (_, params) =>
        client.invalidateQueries(clientOperatorsQueryKeys.get.list(projectId)),
    }
  );
};

export const {
  useDeleteOperatorRoleMutation,
  useOperatorRoleListQuery,
  useUpdateOperatorRoleMutation,
  useCreateOperatorRoleMutation,
  clientOperatorRoleQueryKeys,
} = makeQueryAndMutationForCrudApi(
  'OperatorRole',
  ['operators-roles'],
  OperatorsRolesClientApi,
  (args) => [args.projectId]
);
export const useOperatorRoleQuery = (roleId: OperatorRole['id']) => {
  const operatorRoleApi = useApi(OperatorsRolesClientApi);
  return useQuery([...clientOperatorRoleQueryKeys.root, 'one', roleId], () =>
    operatorRoleApi.getOne({ roleId })
  );
};
