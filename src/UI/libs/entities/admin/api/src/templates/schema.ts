import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { TemplateBase, Template, Context, Component } from './types';

const makeSchema = <T>(segment: string) => {
  return {
    get: createApiAction<T[], void>({
      method: HttpMethod.GET,
      url: `/v1/template-renderer/${segment}/`,
    }),

    save: createApiAction<void, T[]>({
      method: HttpMethod.PATCH,
      url: `/v1/template-renderer/${segment}/`,
      data: (req) => req,
    }),

    delete: createApiAction<void, { id: TemplateBase['id'] }>({
      method: HttpMethod.DELETE,
      url: ({ id }) => `/v1/template-renderer/${segment}/${id}`,
    }),
  };
};

export const TemplateAdminApiSchema = makeSchema<Template>('templates');
export const ContextAdminApiSchema = makeSchema<Context>('contexts');
export const ComponentAdminApiSchema = makeSchema<Component>('components');
