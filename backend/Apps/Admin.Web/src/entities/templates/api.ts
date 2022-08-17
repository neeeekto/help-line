import { httpClient, HttpClient } from "@core/http";
import { Component, Context, Template, TemplateItem } from "./types";
import { Description } from "@entities/meta.types";

export interface TemplateApi<TEntity extends TemplateItem> {
  get: () => Promise<TEntity[]>;
  save: (data: TEntity) => Promise<void>;
  delete: (id: TemplateItem["id"]) => Promise<void>;
}

const makeApi = <T extends TemplateItem>(http: HttpClient, segment: string) =>
  ({
    get: () =>
      http.get<T[]>(`/api/v1/template-renderer/${segment}`).then((x) => x.data),
    save: (data: T) => http.patch(`/api/v1/template-renderer/${segment}`, data),
    delete: (id: TemplateItem["id"]) =>
      http.delete(`/api/v1/template-renderer/${segment}/${id}`),
  } as TemplateApi<T>);

const makeDescriptionApi = (http: HttpClient) => ({
  get: () =>
    http
      .get<Record<string, Description>>(
        `/api/v1/template-renderer/data-descriptions`
      )
      .then((x) => x.data),
});

export const templateApi = makeApi<Template>(httpClient, "templates");
export const contextApi = makeApi<Context>(httpClient, "contexts");
export const componentApi = makeApi<Component>(httpClient, "components");
export const templateDescriptionApi = makeDescriptionApi(httpClient);
