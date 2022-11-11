import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { MigrationAdminApi } from '@help-line/api/admin';
import { WithType } from '@help-line/api/share';
import { useApiClient } from '@help-line/modules/api';

export const adminMigrationsQueryKeys = {
  root: ['admin', 'migrations'] as const,
  list: () => [...adminMigrationsQueryKeys.root, 'list'] as const,
  params: () => [...adminMigrationsQueryKeys.root, 'params'] as const,
};

export const useMigrationsQuery = () => {
  const api = useApiClient(MigrationAdminApi);
  return useQuery(adminMigrationsQueryKeys.list(), api.get);
};

export const useMigrationsParamsQuery = () => {
  const api = useApiClient(MigrationAdminApi);
  return useQuery(adminMigrationsQueryKeys.params(), api.getParamsDescriptions);
};

export const useRunMigrationMutation = (migration: string) => {
  const client = useQueryClient();
  const api = useApiClient(MigrationAdminApi);
  return useMutation(
    [...adminMigrationsQueryKeys.root, 'run'],
    (params?: WithType<string>) => api.run(migration, params),
    {
      onSuccess: () => {
        return client.invalidateQueries(adminMigrationsQueryKeys.list());
      },
    }
  );
};
