import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import {
  ProjectAdminApi,
  Project,
  ProjectData,
  CreateProjectData,
} from '@help-line/entities/admin/api';
import { createQueryKeys } from '@help-line/modules/query';
import { useInjection } from 'inversify-react';

export const adminProjectsQueryKeys = createQueryKeys(
  ['api', 'admin', 'projects'],
  ({ makeKey }) => ({
    list: () => makeKey('list'),
  })
);

export const useProjectsQuery = () => {
  const api = useInjection(ProjectAdminApi);
  return useQuery(adminProjectsQueryKeys.list(), () => api.get());
};

export const useCreateProjectMutation = () => {
  const client = useQueryClient();
  const api = useInjection(ProjectAdminApi);

  return useMutation(
    [...adminProjectsQueryKeys.root, 'create'],
    (data: CreateProjectData) => api.create(data),
    {
      onSuccess: () => {
        return client.invalidateQueries(adminProjectsQueryKeys.list());
      },
    }
  );
};

export const useUpdateProjectMutation = (projectId: Project['id']) => {
  const client = useQueryClient();
  const api = useInjection(ProjectAdminApi);

  return useMutation(
    [...adminProjectsQueryKeys.root, 'update', projectId],
    (data: ProjectData) => api.update({ projectId, data }),
    {
      onSuccess: () => {
        return client.invalidateQueries(adminProjectsQueryKeys.list());
      },
    }
  );
};

export const useActivateProjectMutation = (projectId: string) => {
  const client = useQueryClient();
  const api = useInjection(ProjectAdminApi);

  return useMutation(
    [...adminProjectsQueryKeys.root, 'activate', projectId],
    () => api.activate({ projectId }),
    {
      onSuccess: () => {
        return client.invalidateQueries(adminProjectsQueryKeys.list());
      },
    }
  );
};

export const useDeactivateProjectMutation = (projectId: string) => {
  const client = useQueryClient();
  const api = useInjection(ProjectAdminApi);

  return useMutation(
    [...adminProjectsQueryKeys.root, 'deactivate', projectId],
    () => api.activate({ projectId }),
    {
      onSuccess: () => {
        return client.invalidateQueries(adminProjectsQueryKeys.list());
      },
    }
  );
};

export const useToggleProjectMutation = (projectId: string) => {
  const client = useQueryClient();
  const api = useInjection(ProjectAdminApi);

  return useMutation(
    [...adminProjectsQueryKeys.root, 'toggle', projectId],
    (isActive: boolean) =>
      isActive ? api.deactivate({ projectId }) : api.activate({ projectId }),
    {
      onSuccess: () => {
        return client.invalidateQueries(adminProjectsQueryKeys.list());
      },
    }
  );
};
