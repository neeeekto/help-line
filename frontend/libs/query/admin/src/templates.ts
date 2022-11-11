import {
  ComponentAdminApi,
  ContextAdminApi,
  TemplateAdminApi,
  TemplateDescriptionAdminApi,
  TemplateItem,
  TemplateBaseApi,
} from '@help-line/api/admin';
import {
  useMutation,
  UseMutationResult,
  useQuery,
  useQueryClient,
  UseQueryResult,
} from '@tanstack/react-query';
import { HttpClient } from '@help-line/http';
import { useApiClient } from '@help-line/modules/api';

export interface TemplateItemQueries<T extends TemplateItem = TemplateItem> {
  useListQuery: () => UseQueryResult<T[]>;
  useSaveMutation: (id: T['id']) => UseMutationResult<unknown, unknown, T>;
  useSaveAllMutation: () => UseMutationResult<unknown, unknown, T[]>;
  useDeleteMutation: (id: T['id']) => UseMutationResult;
}

const makeQueriesAndMutationFactory = <T extends TemplateItem = TemplateItem>(
  apiCctor: new (http: HttpClient) => TemplateBaseApi<T>,
  key: string
) => {
  const result: TemplateItemQueries<T> = {
    useListQuery: () => {
      const api = useApiClient(apiCctor);
      return useQuery(['admin', 'template', key], api.get, {});
    },
    useSaveAllMutation: () => {
      const client = useQueryClient();
      const api = useApiClient(apiCctor);
      return useMutation(
        ['admin', 'template', key, 'save'],
        (items: T[]) =>
          Promise.all(items.map((i) => api.save(i))).then(() => {}),
        {
          onSuccess: (data, variables, context) => {
            client.setQueryData<T[]>(['admin', 'template', key], (list) => {
              const result: T[] = list || [];
              for (const variable of variables) {
                const inx = result.findIndex((x) => x.id === variable.id);
                if (inx === -1) {
                  result.push(variable);
                } else {
                  result[inx] = variable;
                }
              }
              return result;
            });
          },
        }
      );
    },
    useSaveMutation: (id: string) => {
      const client = useQueryClient();
      const api = useApiClient(apiCctor);
      return useMutation(['admin', 'template', key, 'save', id], api.save, {
        onSuccess: (data, variables, context) => {
          client.setQueryData<T[]>(['admin', 'template', key], (list) => {
            if (list?.some((x) => x.id === id)) {
              return list?.map((x) => (x.id === id ? variables : x));
            }
            return [...(list ?? []), variables];
          });
        },
      });
    },
    useDeleteMutation: (id: string) => {
      const client = useQueryClient();
      const api = useApiClient(apiCctor);
      return useMutation(
        ['admin', 'template', key, 'delete', id],
        () => api.delete(id),
        {
          onSuccess: (data, variables, context) => {
            client.setQueryData<T[]>(['admin', 'template', key], (list) => {
              return list?.filter((x) => x.id !== id) ?? [];
            });
          },
        }
      );
    },
  };
  return () => result;
};

export const useTemplatesQueries = makeQueriesAndMutationFactory(
  TemplateAdminApi,
  'templates'
);
export const useContextQueries = makeQueriesAndMutationFactory(
  ContextAdminApi,
  'contexts'
);
export const useComponentQueries = makeQueriesAndMutationFactory(
  ComponentAdminApi,
  'components'
);

export const useTemplateDescription = () => {
  const api = useApiClient(TemplateDescriptionAdminApi);
  return useQuery(['admin', 'template', 'descriptions'], api.get);
};
