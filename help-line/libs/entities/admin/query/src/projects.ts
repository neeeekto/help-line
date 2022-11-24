import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import {
  ProjectAdminApi,
  Project,
  ProjectData,
} from '@help-line/entities/admin/api';
import { useApi } from '@help-line/modules/api';

export const adminProjectsQueryKeys = {
  root: ['admin', 'projects'] as const,
  list: () => [...adminProjectsQueryKeys.root, 'list'] as const,
};

export const useProjectsQuery = () => {
  const api = useApi(ProjectAdminApi);
  return useQuery(adminProjectsQueryKeys.list(), () => api.get());
};

export const useCreateProjectMutation = () => {
  const client = useQueryClient();
  const api = useApi(ProjectAdminApi);

  return useMutation([...adminProjectsQueryKeys.root, 'create'], api.create, {
    onSuccess: () => {
      return client.invalidateQueries(adminProjectsQueryKeys.list());
    },
  });
};

export const useUpdateProjectMutation = (projectId: Project['id']) => {
  const client = useQueryClient();
  const api = useApi(ProjectAdminApi);

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
  const api = useApi(ProjectAdminApi);

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
  const api = useApi(ProjectAdminApi);

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
  const api = useApi(ProjectAdminApi);

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
