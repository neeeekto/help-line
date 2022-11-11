import { useQuery } from "react-query";
import { projectApi } from "@entities/helpdesk/projects/api";

export const useProjectsQueries = () => {
  return useQuery(["projects", "list"], projectApi.get);
};
