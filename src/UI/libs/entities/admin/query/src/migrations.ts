import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { MigrationAdminApi } from '@help-line/entities/admin/api';
import { WithType } from '@help-line/entities/share';
import { createQueryKeys } from '@help-line/modules/query';
import { useInjection } from 'inversify-react';

export const adminMigrationsQueryKeys = createQueryKeys(
  ['api', 'admin', 'migrations'],
  ({ makeKey }) => ({
    list: () => makeKey('list'),
    params: () => makeKey('params'),
  })
);

export const useMigrationsQuery = () => {
  const api = useInjection(MigrationAdminApi);
  return useQuery(adminMigrationsQueryKeys.list(), () => api.get());
};

export const useMigrationsParamsQuery = () => {
  const api = useInjection(MigrationAdminApi);
  return useQuery(adminMigrationsQueryKeys.params(), () =>
    api.getParamsDescriptions()
  );
};

export const useRunMigrationMutation = (migration: string) => {
  const client = useQueryClient();
  const api = useInjection(MigrationAdminApi);
  return useMutation(
    [...adminMigrationsQueryKeys.root, 'run'],
    (params?: WithType<string>) => api.run({ migration, params }),
    {
      onSuccess: () => {
        return client.invalidateQueries(adminMigrationsQueryKeys.list());
      },
    }
  );
};
