import { useMutation, useQuery, useQueryClient } from "react-query";
import { IRudApi } from "@core/http";

export const makeQueriesAndMutationForRudApi = <
  TView,
  TUpdate,
  TKey extends string | number,
  TExtend = {}
>(
  api: IRudApi<TView, TUpdate, TKey>,
  namespaceFactory: () => any[],
  updater?: (key: TKey, data: TUpdate, items: TView[]) => TView[],
  remover?: (key: TKey, items: TView[]) => TView[],
  extend?: TExtend
) => {
  const makeNamespace = (...keys: any[]) => [...namespaceFactory(), ...keys];
  const result = {
    useListQuery: () => useQuery(makeNamespace("list"), () => api.get()),
    useSaveMutation: (key: TKey) => {
      const client = useQueryClient();
      return useMutation(
        makeNamespace("save", key),
        async (data: { key: TKey; data: TUpdate }) =>
          api.save(data.data, data.key),
        {
          onSuccess: async (data, variables) => {
            if (updater) {
              client.setQueryData<TView[]>(makeNamespace("list"), (data) =>
                updater(variables.key, variables.data, data || [])
              );
            } else {
              await client.invalidateQueries(makeNamespace("list"));
            }
          },
        }
      );
    },
    useRemoveMutation: (key: TKey) => {
      const client = useQueryClient();
      return useMutation(makeNamespace("remove", key), api.delete, {
        onSuccess: async (data, variables) => {
          if (remover) {
            client.setQueryData<TView[]>(makeNamespace("list"), (items) =>
              remover(variables, items || [])
            );
          } else {
            await client.invalidateQueries(makeNamespace("list"));
          }
        },
      });
    },
    ...extend,
  };
  return () => result as typeof result & TExtend;
};
