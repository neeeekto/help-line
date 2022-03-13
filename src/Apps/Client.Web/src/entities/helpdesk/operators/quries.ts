import { useMutation, useQuery, useQueryClient } from "react-query";
import { operatorApi, operatorRoleApi } from "./api";
import { Ticket } from "@entities/helpdesk/tickets";
import { Operator, OperatorRoleData } from "@entities/helpdesk/operators/types";

export const operatorsQueryKeys = {
  root: "operators",
  view: "view",
};

export const useOperatorsViewQuery = (projectId: string) => {
  return useQuery(
    [operatorsQueryKeys.root, operatorsQueryKeys.view, "list", projectId],
    () => operatorApi.getListView()
  );
};

export const useOperatorViewQuery = (operatorId: string) => {
  return useQuery(
    [operatorsQueryKeys.root, operatorsQueryKeys.view, operatorId],
    () => operatorApi.getOneView(operatorId)
  );
};

export const useMyOperatorQuery = () => {
  return useQuery([operatorsQueryKeys.root, "me"], operatorApi.getMe);
};

export const useChangeFavoriteMutation = () => {
  const client = useQueryClient();
  return useMutation(
    [operatorsQueryKeys.root, "favorite", "change"],
    (data: { ticketId: Ticket["id"]; needAdd: boolean }) => {
      return data.needAdd
        ? operatorApi.addFavorite(data.ticketId)
        : operatorApi.removeFavorite(data.ticketId);
    },
    {
      onSuccess: (_, params) => {
        client.invalidateQueries([operatorsQueryKeys.root, "me"]);
      },
    }
  );
};

export const useOperatorsQuery = () => {
  return useQuery([operatorsQueryKeys.root, "list"], () =>
    operatorApi.getList()
  );
};

export const useOperatorQuery = (operatorId: Operator["id"]) => {
  return useQuery([operatorsQueryKeys.root, operatorId], () =>
    operatorApi.getOne(operatorId)
  );
};

export const useSetOperatorRoleMutation = (operatorId: Operator["id"]) => {
  const client = useQueryClient();
  return useMutation(
    [operatorsQueryKeys.root, operatorId, "roles", "set"],
    (roles: string[]) => operatorApi.setOperatorRoles(operatorId, roles),
    {
      onSuccess: (_, params) => {
        client.invalidateQueries([operatorsQueryKeys.root]);
      },
    }
  );
};

const rolesQueryKeys = {
  root: "operator-roles",
};

export const useOperatorRolesQuery = () =>
  useQuery([rolesQueryKeys.root, "list"], operatorRoleApi.get);

export const useOperatorRoleQuery = (roleId: string) =>
  useQuery([rolesQueryKeys.root, "one", roleId], () =>
    operatorRoleApi.getOne(roleId)
  );

export const useOperatorRoleCreateMutations = () => {
  const client = useQueryClient();
  return useMutation(
    [rolesQueryKeys.root, "create"],
    (data: OperatorRoleData) => operatorRoleApi.add(data),
    {
      onSuccess: async () => {
        await client.invalidateQueries([rolesQueryKeys.root]);
      },
    }
  );
};

export const useOperatorRoleUpdateMutations = (roleId: string) => {
  const client = useQueryClient();
  return useMutation(
    [rolesQueryKeys.root, "update", roleId],
    (data: OperatorRoleData) => operatorRoleApi.update(roleId, data),
    {
      onSuccess: async () => {
        await client.invalidateQueries([rolesQueryKeys.root]);
      },
    }
  );
};

export const useOperatorRoleDeleteMutations = (roleId: string) => {
  const client = useQueryClient();
  return useMutation(
    [rolesQueryKeys.root, "delete", roleId],
    () => operatorRoleApi.delete(roleId),
    {
      onSuccess: async () => {
        await client.invalidateQueries([rolesQueryKeys.root]);
      },
    }
  );
};
