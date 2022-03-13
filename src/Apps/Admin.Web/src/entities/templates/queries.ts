import {
  componentApi,
  contextApi,
  templateApi,
  TemplateApi,
  templateDescriptionApi,
} from "./api";
import {
  useMutation,
  UseMutationResult,
  useQuery,
  useQueryClient,
  UseQueryResult,
} from "react-query";
import { TemplateItem } from "@entities/templates/types";

export interface TemplateItemQueries<T extends TemplateItem = TemplateItem> {
  useListQuery: () => UseQueryResult<T[]>;
  useSaveMutation: (id: T["id"]) => UseMutationResult<void, any, T>;
  useSaveAllMutation: () => UseMutationResult<void, any, T[]>;
  useDeleteMutation: (id: T["id"]) => UseMutationResult<void>;
}

const makeQueriesAndMutationFactory = <T extends TemplateItem = TemplateItem>(
  api: TemplateApi<T>,
  key: string
) => {
  const result: TemplateItemQueries<T> = {
    useListQuery: () => useQuery(["template", key], api.get, {}),
    useSaveAllMutation: () => {
      const client = useQueryClient();
      return useMutation(
        ["template", key, "save"],
        (items: T[]) =>
          Promise.all(items.map((i) => api.save(i))).then(() => {}),
        {
          onSuccess: (data, variables, context) => {
            client.setQueryData<T[]>(["template", key], (list) => {
              const result: T[] = list || [];
              for (let variable of variables) {
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
      return useMutation(["template", key, "save", id], api.save, {
        onSuccess: (data, variables, context) => {
          client.setQueryData<T[]>(["template", key], (list) => {
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
      return useMutation(
        ["template", key, "delete", id],
        () => api.delete(id),
        {
          onSuccess: (data, variables, context) => {
            client.setQueryData<T[]>(["template", key], (list) => {
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
  templateApi,
  "templates"
);
export const useContextQueries = makeQueriesAndMutationFactory(
  contextApi,
  "contexts"
);
export const useComponentQueries = makeQueriesAndMutationFactory(
  componentApi,
  "components"
);

export const useTemplateDescription = () =>
  useQuery(["template", "descriptions"], templateDescriptionApi.get);
