import { useMutation, useQuery, useQueryClient } from "react-query";
import { useOperatorApi, useOperatorRoleApi } from "./api";
import { Ticket } from "@entities/helpdesk/tickets";
import { Operator, OperatorRoleData } from "@entities/helpdesk/operators/types";

export const operatorsQueryKeys = {
  root: "operators",
  view: "view",
};

export const useOperatorsViewQuery = (projectId: string) => {
  const operatorApi = useOperatorApi();
  return useQuery(
    [projectId, operatorsQueryKeys.root, operatorsQueryKeys.view, "list"],
    () => operatorApi.getListView()
  );
};

export const useOperatorViewQuery = (operatorId: string) => {
  const operatorApi = useOperatorApi();
  return useQuery(
    [operatorsQueryKeys.root, operatorsQueryKeys.view, operatorId],
    () => operatorApi.getOneView(operatorId)
  );
};

export const useMyOperatorQuery = () => {
  const operatorApi = useOperatorApi();
  return useQuery([operatorsQueryKeys.root, "me"], operatorApi.getMe);
};

export const useChangeFavoriteMutation = () => {
  const client = useQueryClient();
  const operatorApi = useOperatorApi();
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
  const operatorApi = useOperatorApi();
  return useQuery([operatorsQueryKeys.root, "list"], () =>
    operatorApi.getList()
  );
};

export const useOperatorQuery = (operatorId: Operator["id"]) => {
  const operatorApi = useOperatorApi();
  return useQuery([operatorsQueryKeys.root, operatorId], () =>
    operatorApi.getOne(operatorId)
  );
};

export const useSetOperatorRoleMutation = (operatorId: Operator["id"]) => {
  const client = useQueryClient();
  const operatorApi = useOperatorApi();
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

export const useOperatorRolesQuery = () => {
  const operatorRoleApi = useOperatorRoleApi();
  return useQuery([rolesQueryKeys.root, "list"], operatorRoleApi.get);
};

export const useOperatorRoleQuery = (roleId: string) => {
  const operatorRoleApi = useOperatorRoleApi();
  return useQuery([rolesQueryKeys.root, "one", roleId], () =>
    operatorRoleApi.getOne(roleId)
  );
};

export const useOperatorRoleCreateMutations = () => {
  const client = useQueryClient();
  const operatorRoleApi = useOperatorRoleApi();
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
  const operatorRoleApi = useOperatorRoleApi();
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
  const operatorRoleApi = useOperatorRoleApi();
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
