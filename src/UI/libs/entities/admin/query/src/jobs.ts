import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { JobsAdminApi, Job, JobData } from '@help-line/entities/admin/api';
import { useApi } from '@help-line/modules/api';
import { createQueryKeys } from '@help-line/modules/query';

export const adminJobsQueryKeys = createQueryKeys(
  ['api', 'admin', 'jobs'],
  ({ makeKey, root }) => ({
    list: () => makeKey('list'),
    tasks: () => makeKey('tasks'),
    state: createQueryKeys(makeKey('states'), ({ makeKey }) => ({
      for: (jobIds: Array<Job['id']>) => makeKey(jobIds),
    })),
  })
);

export const useJobsQuery = () => {
  const api = useApi(JobsAdminApi);
  return useQuery(adminJobsQueryKeys.list(), () => api.get());
};

export const useJobTasksQuery = () => {
  const api = useApi(JobsAdminApi);
  return useQuery(adminJobsQueryKeys.tasks(), () => api.getTasks());
};

export const useJobsStateQuery = (jobIds: Array<Job['id']>) => {
  const api = useApi(JobsAdminApi);
  return useQuery(adminJobsQueryKeys.state.for(jobIds), () =>
    api.getState({ jobIds })
  );
};

export const useCreateJobMutation = (task: string) => {
  const client = useQueryClient();
  const api = useApi(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'create', task],
    (data: JobData) => api.create({ task, data }),
    {
      onSuccess: () =>
        Promise.all([
          client.invalidateQueries(adminJobsQueryKeys.list()),
          client.invalidateQueries(adminJobsQueryKeys.state.root),
        ]),
    }
  );
};

export const useUpdateJobMutation = (jobId: Job['id']) => {
  const client = useQueryClient();
  const api = useApi(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'update', jobId],
    (data: JobData) => api.update({ jobId, data }),
    {
      onSuccess: () =>
        Promise.all([
          client.invalidateQueries(adminJobsQueryKeys.list()),
          client.invalidateQueries(adminJobsQueryKeys.state.root),
        ]),
    }
  );
};

export const useDeleteJobMutation = (jobId: Job['id']) => {
  const client = useQueryClient();
  const api = useApi(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'delete', jobId],
    () => api.delete({ jobId }),
    {
      onSuccess: (data, variables, context) => {
        client.setQueryData(adminJobsQueryKeys.list(), (list?: Job[]) => {
          return (list || []).filter((x) => x.id !== jobId);
        });
      },
    }
  );
};

export const useToggleJobMutation = (jobId: Job['id']) => {
  const client = useQueryClient();
  const api = useApi(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'toggle', jobId],
    (enable: boolean) => api.toggle({ jobId, enable }),
    {
      onSuccess: async (data, variables, context) => {
        client.setQueryData(adminJobsQueryKeys.list(), (list?: Job[]) => {
          (list || []).forEach((x) => {
            if (x.id === jobId) {
              x.enabled = variables;
            }
          });
          return list || [];
        });
        await client.refetchQueries(adminJobsQueryKeys.state.root);
      },
    }
  );
};

export const useToggleJobsMutation = (jobsIds: Job['id'][]) => {
  const client = useQueryClient();
  const api = useApi(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'toggle', jobsIds],
    (enable: boolean) =>
      Promise.all(
        jobsIds.map((jobId) =>
          api.toggle({ jobId, enable }).then((res) => {
            client.setQueryData(adminJobsQueryKeys.list(), (list?: Job[]) => {
              (list || []).forEach((x) => {
                if (jobsIds.includes(x.id)) {
                  x.enabled = enable;
                }
              });
              return list || [];
            });
          })
        )
      ),
    {
      onSuccess: () => client.refetchQueries(adminJobsQueryKeys.state.root),
    }
  );
};

export const useFireJobMutation = (jobId: Job['id']) => {
  const api = useApi(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'fire', jobId],
    () => api.fire({ jobId }),
    {}
  );
};
