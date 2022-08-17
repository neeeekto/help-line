import { useMutation, useQueryClient } from "react-query";
import { dictionaryTagDescriptionApi, dictionaryTagsApi } from "./api";
import { addOrUpdate } from "@shared/utils/list-utils";
import { makeQueriesAndMutationForRudApi } from "@core/queries/factories";
import { Project } from "@entities/helpdesk/projects";

export const useTagsDescriptionQueries = (projectId: Project["id"]) =>
  makeQueriesAndMutationForRudApi(
    dictionaryTagDescriptionApi,
    () => [projectId, "descriptions"],
    undefined,
    (tag, items) => items.filter((x) => x.tag !== tag)
  )();
export const useTagsQueries = (projectId: Project["id"]) =>
  makeQueriesAndMutationForRudApi(
    dictionaryTagsApi,
    () => [projectId, "tags"],
    (tag, data, items) =>
      addOrUpdate(
        items,
        (x) => x.key === tag,
        (x) => ({
          ...x,
          enabled: data,
        }),
        () => ({ key: tag, enabled: data })
      ),
    (tag, items) => items.filter((x) => x.key !== tag),
    {
      useSaveManyMutation: (tags?: string) => {
        const client = useQueryClient();
        return useMutation(
          [projectId, "tags", "save", tags],
          async (data: { tags: string[]; enabled: boolean }) =>
            dictionaryTagsApi.saveMany(data.tags, data.enabled),
          {
            onSuccess: async (data, variables) => {
              await client.invalidateQueries([projectId, "tags"]);
            },
          }
        );
      },
    }
  )();
