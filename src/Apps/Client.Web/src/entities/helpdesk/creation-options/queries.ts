import {
  creationOptionsPlatformApi,
  creationOptionsProblemAndThemesApi,
} from "./api";
import { addOrUpdate } from "@shared/utils/list-utils";
import { makeQueriesAndMutationForRudApi } from "@core/queries/factories";
import { Project } from "@entities/helpdesk/projects";

export const useCreationOptionsPlatformQueries = (projectId: Project["id"]) =>
  makeQueriesAndMutationForRudApi(
    creationOptionsPlatformApi,
    () => [projectId, "platform"],
    (key, data, items) =>
      addOrUpdate(
        items,
        (x) => x.key === key,
        (x) => ({
          ...x,
          name: data,
        }),
        () => ({ key: key, name: data, icon: "", sort: 0 })
      ),
    (tag, items) => items.filter((x) => x.key !== tag)
  )();
export const useCreationOptionsProblemAndThemesQueries = (
  projectId: Project["id"]
) =>
  makeQueriesAndMutationForRudApi(
    creationOptionsProblemAndThemesApi,
    () => [projectId, "problemAndThemes"],
    (tag, data, items) =>
      addOrUpdate(
        items,
        (x) => x.tag === tag,
        () => data,
        () => data
      ),
    (tag, items) => items.filter((x) => x.tag !== tag)
  )();
