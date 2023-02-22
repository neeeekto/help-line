import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { createQueryKeys } from '@help-line/modules/query';
import {
  Project,
  User,
  UserData,
  UserInfo,
  UsersClientApi,
} from '@help-line/entities/client/api';
import { ROOT_QUERY_KEY } from '../constants';
import { clientOperatorsQueryKeys } from '../helpdesk/operators';
import { useInjection } from 'inversify-react';

export const clientUsersQueryKeys = createQueryKeys(
  [ROOT_QUERY_KEY, 'users'],
  ({ makeKey }) => ({
    list: createQueryKeys(makeKey('list'), ({ makeKey }) => ({
      byProject: (projectId?: Project['id']) => makeKey('list', projectId),
    })),
    detail: createQueryKeys(makeKey('details'), ({ makeKey }) => ({
      byUser: (userId: User['id']) => makeKey(userId),
    })),
  })
);

export const useUsersQuery = (projectId?: Project['id']) => {
  const api = useInjection(UsersClientApi);
  return useQuery(clientUsersQueryKeys.list.byProject(projectId), () =>
    api.get({ projectId })
  );
};

export const useUserQuery = (userId: User['id']) => {
  const api = useInjection(UsersClientApi);

  return useQuery(clientUsersQueryKeys.detail.byUser(userId), () =>
    api.getById({ userId })
  );
};

export const useCreateUserMutation = () => {
  const client = useQueryClient();
  const api = useInjection(UsersClientApi);

  return useMutation((data: UserData) => api.create({ data }), {
    onSuccess: () =>
      Promise.all([
        client.invalidateQueries([clientUsersQueryKeys.list.root]),
        client.invalidateQueries([clientOperatorsQueryKeys.root]),
      ]),
  });
};

export const useUpdateInfoUserMutation = (userId: User['id']) => {
  const client = useQueryClient();
  const api = useInjection(UsersClientApi);

  return useMutation(
    [...clientUsersQueryKeys.root, 'update', userId],
    (info: UserInfo) => api.updateInfo({ userId, info }),
    {
      onSuccess: (_, info, context) =>
        Promise.all([
          client.invalidateQueries([clientUsersQueryKeys.list.root]),
          client.invalidateQueries([
            clientUsersQueryKeys.detail.byUser(userId),
          ]),
        ]),
    }
  );
};

export const useRemoveUserMutation = (userId: User['id']) => {
  const client = useQueryClient();
  const api = useInjection(UsersClientApi);

  return useMutation(
    [...clientUsersQueryKeys.root, 'delete', userId],
    () => api.delete({ userId }),
    {
      onSuccess: () =>
        Promise.all([
          client.invalidateQueries([
            clientUsersQueryKeys.detail.byUser(userId),
          ]),
          client.invalidateQueries([clientOperatorsQueryKeys.root]),
        ]),
    }
  );
};
