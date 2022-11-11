import { httpClient, makeRudApi } from "@core/http";
import { Tag, TagDescription, TagDescriptionItem } from "./types";

export const dictionaryTagsApi = {
  ...makeRudApi<Tag, boolean>(httpClient, "/api/v1/hd/tags"),
  saveMany: (tags: string[], enabled: boolean) =>
    httpClient
      .post<void>(`/api/v1/hd/tags`, { tags, enabled })
      .then((x) => x.data),
};
export const dictionaryTagDescriptionApi = makeRudApi<
  TagDescriptionItem,
  TagDescription
>(httpClient, "/api/v1/hd/tags/description");
