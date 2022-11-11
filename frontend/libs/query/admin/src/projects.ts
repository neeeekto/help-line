import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { ProjectAdminApi, Project, ProjectData } from '@help-line/api/admin';
import { useApiClient } from '@help-line/modules/api';

export const adminProjectsQueryKeys = {
  root: ['admin', 'projects'] as const,
  list: () => [...adminProjectsQueryKeys.root, 'list'] as const,
};

export const useProjectsQuery = () => {
  const api = useApiClient(ProjectAdminApi);
  return useQuery(adminProjectsQueryKeys.list(), api.get);
};

export const useProjectCreateMutation = () => {
  const client = useQueryClient();
  const api = useApiClient(ProjectAdminApi);

  return useMutation([...adminProjectsQueryKeys.root, 'create'], api.create, {
    onSuccess: () => {
      return client.invalidateQueries(adminProjectsQueryKeys.list());
    },
  });
};

export const useProjectUpdateMutation = () => {
  const client = useQueryClient();
  const api = useApiClient(ProjectAdminApi);

  return useMutation(
    [...adminProjectsQueryKeys.root, 'update'],
    (args: { projectId: Project['id']; data: ProjectData }) =>
      api.update(args.projectId, args.data),
    {
      onSuccess: () => {
        return client.invalidateQueries(adminProjectsQueryKeys.list());
      },
    }
  );
};

export const useProjectActivateToggleMutation = (projectId: string) => {
  const client = useQueryClient();
  const api = useApiClient(ProjectAdminApi);

  return useMutation(
    [...adminProjectsQueryKeys.root, 'activate', projectId],
    (isActive: boolean) =>
      isActive ? api.deactivate(projectId) : api.activate(projectId),
    {
      onSuccess: () => {
        return client.invalidateQueries(adminProjectsQueryKeys.list());
      },
    }
  );
};
