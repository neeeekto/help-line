import { useMutation, useQuery, useQueryClient } from "react-query";
import { jobsApi } from "./api";
import { Job, JobData } from "./types";

export const useJobsQuery = () => {
  return useQuery(["jobs", "list"], jobsApi.get);
};

export const useJobTasksQuery = () => {
  return useQuery(["jobs", "tasks"], jobsApi.getTasks);
};

export const useJobsStateQuery = (jobIds: Array<Job["id"]>) => {
  return useQuery(["jobs", "state", jobIds], () => jobsApi.getState(jobIds));
};

export const useCreateJobMutation = (task: string) => {
  const client = useQueryClient();
  return useMutation(
    ["jobs", "create", task],
    (data: JobData) => jobsApi.create(task, data),
    {
      onSuccess: async (data, variables, context) => {
        const job = await jobsApi.getById(data);
        client.setQueryData(["jobs", "list"], (list?: Job[]) => {
          return [...(list || []), job];
        });
        client.invalidateQueries(["jobs", "state"]);
      },
    }
  );
};

export const useUpdateJobMutation = (jobId: Job["id"]) => {
  const client = useQueryClient();
  return useMutation(
    ["jobs", "update", jobId],
    (data: JobData) => jobsApi.update(jobId, data),
    {
      onSuccess: async (data, variables, context) => {
        const job = await jobsApi.getById(jobId);
        client.setQueryData(["jobs", "list"], (list?: Job[]) => {
          return (list || []).map((x) => (x.id === jobId ? job : x));
        });
        client.invalidateQueries(["jobs", "state"]);
      },
    }
  );
};

export const useDeleteJobMutation = (jobId: Job["id"]) => {
  const client = useQueryClient();
  return useMutation(["jobs", "delete", jobId], () => jobsApi.delete(jobId), {
    onSuccess: (data, variables, context) => {
      client.setQueryData(["jobs", "list"], (list?: Job[]) => {
        return (list || []).filter((x) => x.id !== jobId);
      });
    },
  });
};

export const useToggleJobMutation = (jobId: Job["id"]) => {
  const client = useQueryClient();
  return useMutation(
    ["jobs", "toggle", jobId],
    (enable: boolean) => jobsApi.toggle(jobId, enable),
    {
      onSuccess: async (data, variables, context) => {
        await client.refetchQueries(["jobs", "state"]);
        client.setQueryData(["jobs", "list"], (list?: Job[]) => {
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

export const useToggleJobsMutation = (jobsIds: Job["id"][]) => {
  const client = useQueryClient();
  return useMutation(
    ["jobs", "toggle", jobsIds],
    (enable: boolean) =>
      Promise.all(jobsIds.map((jobId) => jobsApi.toggle(jobId, enable))),
    {
      onSuccess: async (data, variables, context) => {
        await client.refetchQueries(["jobs", "state"]);
        client.setQueryData(["jobs", "list"], (list?: Job[]) => {
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

export const useFireJobMutation = (jobId: Job["id"]) => {
  return useMutation(["jobs", "delete", jobId], () => jobsApi.fire(jobId), {});
};
