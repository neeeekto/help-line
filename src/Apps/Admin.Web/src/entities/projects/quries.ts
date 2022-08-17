import { useQuery, useMutation, useQueryClient } from "react-query";
import { projectApi } from "@entities/projects/api";
import { Project, ProjectData } from "@entities/projects/types";

export const useProjectsQuery = () => {
  return useQuery(["projects", "list"], projectApi.get);
};

export const useProjectCreateMutation = () => {
  const client = useQueryClient();
  return useMutation(["project", "create"], projectApi.create, {
    onSuccess: () => {
      return client.invalidateQueries(["projects"]);
    },
  });
};

export const useProjectUpdateMutation = () => {
  const client = useQueryClient();
  return useMutation(
    ["project", "update"],
    (args: { projectId: Project["id"]; data: ProjectData }) =>
      projectApi.update(args.projectId, args.data),
    {
      onSuccess: () => {
        return client.invalidateQueries(["projects"]);
      },
    }
  );
};

export const useProjectActivateToggleMutation = (projectId: string) => {
  const client = useQueryClient();
  return useMutation(
    ["project", "activate", projectId],
    (isActive: boolean) =>
      isActive
        ? projectApi.deactivate(projectId)
        : projectApi.activate(projectId),
    {
      onSuccess: () => {
        return client.invalidateQueries(["projects"]);
      },
    }
  );
};
