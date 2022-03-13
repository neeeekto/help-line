import { useQuery, useMutation, useQueryClient } from "react-query";
import { migrationApi } from "@entities/migrations/api";
import { WithType } from "@entities/common";

const queryKeys = {
  root: "migrations",
  list: "list",
  params: "params",
};

export const useMigrationsQuery = () => {
  return useQuery([queryKeys.root, queryKeys.list], migrationApi.get);
};

export const useMigrationsParamsQuery = () => {
  return useQuery(
    [queryKeys.root, queryKeys.params],
    migrationApi.getParamsDescriptions
  );
};

export const useRunMigrationMutation = (migration: string) => {
  const client = useQueryClient();
  return useMutation(
    [queryKeys.root, "run"],
    (params?: WithType<string>) => migrationApi.run(migration, params),
    {
      onSuccess: () => {
        return client.invalidateQueries([queryKeys.root, queryKeys.list]);
      },
    }
  );
};
