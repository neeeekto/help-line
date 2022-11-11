import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  JobsAdminApi,
  Job,
  JobData,
  TicketScheduleStatus,
} from '@help-line/api/admin';
import { useApiClient } from '@help-line/modules/api';

export const adminJobsQueryKeys = {
  root: ['admin', 'jobs'] as const,
  list: () => [...adminJobsQueryKeys.root, 'list'] as const,
  tasks: () => [...adminJobsQueryKeys.root, 'tasks'] as const,
  states: () => [...adminJobsQueryKeys.root, 'state'] as const,
  state: (jobIds: Array<Job['id']>) =>
    [...adminJobsQueryKeys.states(), jobIds] as const,
};

export const useJobsQuery = () => {
  const api = useApiClient(JobsAdminApi);
  return useQuery(adminJobsQueryKeys.list(), api.get);
};

export const useJobTasksQuery = () => {
  const api = useApiClient(JobsAdminApi);
  return useQuery(adminJobsQueryKeys.tasks(), api.getTasks);
};

export const useJobsStateQuery = (jobIds: Array<Job['id']>) => {
  const api = useApiClient(JobsAdminApi);
  return useQuery(adminJobsQueryKeys.state(jobIds), () => api.getState(jobIds));
};

export const useCreateJobMutation = (task: string) => {
  const client = useQueryClient();
  const api = useApiClient(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'create', task],
    (data: JobData) => api.create(task, data),
    {
      onSuccess: async (data, variables, context) => {
        const job = await api.getById(data);
        client.setQueryData(adminJobsQueryKeys.list(), (list?: Job[]) => {
          return [...(list || []), job];
        });
        await client.invalidateQueries(adminJobsQueryKeys.states());
      },
    }
  );
};

export const useUpdateJobMutation = (jobId: Job['id']) => {
  const client = useQueryClient();
  const api = useApiClient(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'update', jobId],
    (data: JobData) => api.update(jobId, data),
    {
      onSuccess: async (data, variables, context) => {
        const job = await api.getById(jobId);
        client.setQueryData(adminJobsQueryKeys.list(), (list?: Job[]) => {
          return (list || []).map((x) => (x.id === jobId ? job : x));
        });
        await client.invalidateQueries(adminJobsQueryKeys.states());
      },
    }
  );
};

export const useDeleteJobMutation = (jobId: Job['id']) => {
  const client = useQueryClient();
  const api = useApiClient(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'delete', jobId],
    () => api.delete(jobId),
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
  const api = useApiClient(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'toggle', jobId],
    (enable: boolean) => api.toggle(jobId, enable),
    {
      onSuccess: async (data, variables, context) => {
        await client.refetchQueries(adminJobsQueryKeys.states());
        client.setQueryData(adminJobsQueryKeys.list(), (list?: Job[]) => {
          (list || []).forEach((x) => {
            if (x.id === jobId) {
              x.enabled = variables;
            }
          });
          return list || [];
        });
      },
    }
  );
};

export const useToggleJobsMutation = (jobsIds: Job['id'][]) => {
  const client = useQueryClient();
  const api = useApiClient(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'toggle', jobsIds],
    (enable: boolean) =>
      Promise.all(jobsIds.map((jobId) => api.toggle(jobId, enable))),
    {
      onSuccess: async (data, variables, context) => {
        await client.refetchQueries(adminJobsQueryKeys.states());
        client.setQueryData(adminJobsQueryKeys.list(), (list?: Job[]) => {
          (list || []).forEach((x) => {
            if (jobsIds.includes(x.id)) {
              x.enabled = variables;
            }
          });
          return list || [];
        });
      },
    }
  );
};

export const useFireJobMutation = (jobId: Job['id']) => {
  const api = useApiClient(JobsAdminApi);
  return useMutation(
    [...adminJobsQueryKeys.root, 'delete', jobId],
    () => api.fire(jobId),
    {}
  );
};
