import { createQueryKeys } from '@help-line/modules/query';
import { ROOT_QUERY_KEY } from './constants';
import { useInjection } from 'inversify-react';
import { interfaces } from 'inversify';
import {
  QueryKey,
  useMutation,
  useQuery,
  useQueryClient,
} from '@tanstack/react-query';

export interface IRudApi<TEntity, TSaveData, TId, TShare> {
  get(req: TShare): Promise<TEntity[]>;
  save(params: { id: TId; data: TSaveData } & TShare): Promise<void>;
  delete(params: { id: TId } & TShare): Promise<void>;
}

export const makeQueryAndMutationForRudApi = <
  TEntity,
  TSaveData,
  TId,
  TShareArgs,
  TName extends string
>(
  name: TName,
  key: QueryKey,
  cctor: interfaces.Newable<IRudApi<TEntity, TSaveData, TId, TShareArgs>>,
  shareArgToKeys: (args: TShareArgs) => QueryKey = () => []
) => {
  const queryKeys = createQueryKeys(
    [ROOT_QUERY_KEY, ...key],
    ({ makeKey }) => ({
      list: (args: TShareArgs) => makeKey(...shareArgToKeys(args), 'list'),
    })
  );

  const useListQuery = (args: TShareArgs) => {
    const api = useInjection(cctor);
    return useQuery(queryKeys.list(args), () => api.get(args));
  };

  const useSaveMutation = (params: { id: TId } & TShareArgs) => {
    const api = useInjection(cctor);
    const client = useQueryClient();
    return useMutation(
      [...queryKeys.root, 'save', params.id],
      (data: TSaveData) => api.save({ data, ...params }),
      {
        onSuccess: () => client.invalidateQueries(queryKeys.list(params)),
      }
    );
  };

  const useDeleteMutation = (params: { id: TId } & TShareArgs) => {
    const api = useInjection(cctor);
    const client = useQueryClient();
    return useMutation(
      [...queryKeys.root, 'delete', params.id],
      () => api.delete(params),
      {
        onSuccess: () => client.invalidateQueries(queryKeys.list(params)),
      }
    );
  };

  return {
    [`client${name}QueryKeys`]: queryKeys,
    [`use${name}ListQuery`]: useListQuery,
    [`useSave${name}Mutation`]: useSaveMutation,
    [`useDelete${name}Mutation`]: useDeleteMutation,
  } as {
    [key in `client${TName}QueryKeys`]: typeof queryKeys;
  } & {
    [key in `use${TName}ListQuery`]: typeof useListQuery;
  } & {
    [key in `useSave${TName}Mutation`]: typeof useSaveMutation;
  } & {
    [key in `useDelete${TName}Mutation`]: typeof useDeleteMutation;
  };
};

export interface ICrudApi<TEntity, TCreateData, TUpdateData, TId, TShare> {
  get(req: TShare): Promise<TEntity[]>;
  delete(params: { id: TId } & TShare): Promise<void>;
  create(params: { data: TCreateData } & TShare): Promise<void>;
  update(params: { id: TId; data: TUpdateData } & TShare): Promise<void>;
}

export const makeQueryAndMutationForCrudApi = <
  TEntity,
  TCreateData,
  TUpdateData,
  TId,
  TShareArgs,
  TName extends string
>(
  name: TName,
  key: QueryKey,
  cctor: interfaces.Newable<
    ICrudApi<TEntity, TCreateData, TUpdateData, TId, TShareArgs>
  >,
  shareArgToKeys: (args: TShareArgs) => QueryKey = () => []
) => {
  const queryKeys = createQueryKeys(
    [ROOT_QUERY_KEY, ...key],
    ({ makeKey }) => ({
      list: (args: TShareArgs) => makeKey(...shareArgToKeys(args), 'list'),
    })
  );

  const useListQuery = (args: TShareArgs) => {
    const api = useInjection(cctor);
    return useQuery(queryKeys.list(args), () => api.get(args));
  };

  const useCreateMutation = (params: TShareArgs) => {
    const api = useInjection(cctor);
    const client = useQueryClient();
    return useMutation(
      [...queryKeys.root, 'create'],
      (data: TCreateData) => api.create({ data, ...params }),
      {
        onSuccess: () => client.invalidateQueries(queryKeys.list(params)),
      }
    );
  };

  const useUpdateMutation = (params: { id: TId } & TShareArgs) => {
    const api = useInjection(cctor);
    const client = useQueryClient();
    return useMutation(
      [...queryKeys.root, 'create'],
      (data: TUpdateData) => api.update({ data, ...params }),
      {
        onSuccess: () => client.invalidateQueries(queryKeys.list(params)),
      }
    );
  };

  const useDeleteMutation = (params: { id: TId } & TShareArgs) => {
    const api = useInjection(cctor);
    const client = useQueryClient();
    return useMutation(
      [...queryKeys.root, 'delete', params.id],
      () => api.delete(params),
      {
        onSuccess: () => client.invalidateQueries(queryKeys.list(params)),
      }
    );
  };

  return {
    [`client${name}QueryKeys`]: queryKeys,
    [`use${name}ListQuery`]: useListQuery,
    [`useCreate${name}Mutation`]: useCreateMutation,
    [`useUpdate${name}Mutation`]: useUpdateMutation,
    [`useDelete${name}Mutation`]: useDeleteMutation,
  } as {
    [key in `client${TName}QueryKeys`]: typeof queryKeys;
  } & {
    [key in `use${TName}ListQuery`]: typeof useListQuery;
  } & {
    [key in `useCreate${TName}Mutation`]: typeof useCreateMutation;
  } & {
    [key in `useUpdate${TName}Mutation`]: typeof useUpdateMutation;
  } & {
    [key in `useDelete${TName}Mutation`]: typeof useDeleteMutation;
  };
};
