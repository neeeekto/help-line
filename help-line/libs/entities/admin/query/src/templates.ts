import { createQueryKeys } from '@help-line/modules/query';
import { useApi } from '@help-line/modules/api';
import {
  ContextAdminApi,
  Template,
  TemplateAdminApi,
  Context,
  ComponentAdminApi,
  Component,
} from '@help-line/entities/admin/api';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';

const createTemplateQueryKeys = (segment: string) =>
  createQueryKeys(['admin', 'template-renderer', segment], ({ makeKey }) => ({
    list: () => makeKey('list'),
    save: (id: string) => makeKey('save', id),
    saveList: (id: string[]) => makeKey('saveList', ...id),
    delete: (id: string) => makeKey('delete', id),
  }));

export const adminTemplateQueryKeys = createTemplateQueryKeys('templates');

export const useTemplatesQuery = () => {
  const api = useApi(TemplateAdminApi);
  return useQuery(adminTemplateQueryKeys.list(), () => api.get());
};

export const useTemplateSaveMutation = (id: string) => {
  const client = useQueryClient();
  const api = useApi(TemplateAdminApi);
  return useMutation(
    adminTemplateQueryKeys.save(id),
    (req: Template) => api.save([req]),
    {
      onSuccess: () => client.invalidateQueries(adminTemplateQueryKeys.list()),
    }
  );
};

export const useTemplatesSaveMutation = (ids: string[]) => {
  const client = useQueryClient();
  const api = useApi(TemplateAdminApi);
  return useMutation(
    adminTemplateQueryKeys.saveList(ids),
    (req: Template[]) => api.save(req),
    {
      onSuccess: () => client.invalidateQueries(adminTemplateQueryKeys.list()),
    }
  );
};

export const useTemplateDeleteMutation = (id: string) => {
  const client = useQueryClient();
  const api = useApi(TemplateAdminApi);
  return useMutation(
    adminTemplateQueryKeys.delete(id),
    () => api.delete({ id }),
    {
      onSuccess: () => client.invalidateQueries(adminTemplateQueryKeys.list()),
    }
  );
};

export const adminContextQueryKeys = createTemplateQueryKeys('contexts');

export const useContextsQuery = () => {
  const api = useApi(ContextAdminApi);
  return useQuery(adminContextQueryKeys.list(), () => api.get());
};

export const useContextSaveMutation = (id: string) => {
  const client = useQueryClient();
  const api = useApi(ContextAdminApi);
  return useMutation(
    adminContextQueryKeys.save(id),
    (req: Context) => api.save([req]),
    {
      onSuccess: () => client.invalidateQueries(adminContextQueryKeys.list()),
    }
  );
};

export const useContextsSaveMutation = (ids: string[]) => {
  const client = useQueryClient();
  const api = useApi(ContextAdminApi);
  return useMutation(
    adminContextQueryKeys.saveList(ids),
    (req: Context[]) => api.save(req),
    {
      onSuccess: () => client.invalidateQueries(adminContextQueryKeys.list()),
    }
  );
};

export const useContextDeleteMutation = (id: string) => {
  const client = useQueryClient();
  const api = useApi(ContextAdminApi);
  return useMutation(
    adminContextQueryKeys.delete(id),
    () => api.delete({ id }),
    {
      onSuccess: () => client.invalidateQueries(adminContextQueryKeys.list()),
    }
  );
};

export const adminComponentQueryKeys = createTemplateQueryKeys('components');

export const useComponentsQuery = () => {
  const api = useApi(ComponentAdminApi);
  return useQuery(adminComponentQueryKeys.list(), () => api.get());
};

export const useComponentSaveMutation = (id: string) => {
  const client = useQueryClient();
  const api = useApi(ComponentAdminApi);
  return useMutation(
    adminComponentQueryKeys.save(id),
    (req: Component) => api.save([req]),
    {
      onSuccess: () => client.invalidateQueries(adminComponentQueryKeys.list()),
    }
  );
};

export const useComponentsSaveMutation = (ids: string[]) => {
  const client = useQueryClient();
  const api = useApi(ComponentAdminApi);
  return useMutation(
    adminComponentQueryKeys.saveList(ids),
    (req: Component[]) => api.save(req),
    {
      onSuccess: () => client.invalidateQueries(adminComponentQueryKeys.list()),
    }
  );
};

export const useComponentDeleteMutation = (id: string) => {
  const client = useQueryClient();
  const api = useApi(ComponentAdminApi);
  return useMutation(
    adminComponentQueryKeys.delete(id),
    () => api.delete({ id }),
    {
      onSuccess: () => client.invalidateQueries(adminComponentQueryKeys.list()),
    }
  );
};
