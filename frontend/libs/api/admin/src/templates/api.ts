import { Component, Context, Template, TemplateItem } from './types';
import { ApiBase, HttpClient } from '@help-line/http';
import { Description } from '@help-line/api/share';

export abstract class TemplateBaseApi<T> extends ApiBase {
  private readonly segment: string;

  protected constructor(http: HttpClient, segment: string) {
    super(http);
    this.segment = segment;
  }

  public async get() {
    return this.http
      .get<T[]>(`/v1/template-renderer/${this.segment}/`)
      .then((x) => x.data);
  }
  public async save(data: T) {
    return this.http.patch<void>(
      `/v1/template-renderer/${this.segment}/`,
      data
    );
  }
  public async delete(id: TemplateItem['id']) {
    return this.http.delete(`/v1/template-renderer/${this.segment}/${id}`);
  }
}

export class TemplateAdminApi extends TemplateBaseApi<Template> {
  constructor(http: HttpClient) {
    super(http, 'templates');
  }
}

export class ContextAdminApi extends TemplateBaseApi<Context> {
  constructor(http: HttpClient) {
    super(http, 'contexts');
  }
}

export class ComponentAdminApi extends TemplateBaseApi<Component> {
  constructor(http: HttpClient) {
    super(http, 'components');
  }
}

export class TemplateDescriptionAdminApi extends ApiBase {
  public async get() {
    return this.http
      .get<Record<string, Description>>(
        `/api/v1/template-renderer/data-descriptions`
      )
      .then((x) => x.data);
  }
}
