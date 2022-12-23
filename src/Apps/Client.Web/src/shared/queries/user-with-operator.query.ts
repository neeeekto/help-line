import { Project } from "@entities/helpdesk/projects";
import { useQuery } from "react-query";
import { useUsersQuery } from "@entities/user-access/users/queries";

export const useUserWithOperatorQuery = (projectId: Project["id"]) => {
  const users = useUsersQuery();
};
