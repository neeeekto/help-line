import { useMutation, useQuery, useQueryClient } from "react-query";
import { userApi } from "./api";
import { User, UserData, UserInfo } from "./types";
import { operatorsQueryKeys } from "@entities/helpdesk/operators";
import { Project } from "@entities/helpdesk/projects";

export const useUsersQuery = (projectId?: Project["id"]) => {
  return useQuery(["users", "list", projectId], () => userApi.get(projectId));
};

export const useUserQuery = (userId: User["id"]) => {
  return useQuery(["users", "detail", userId], () =>
    userApi.getById(userId).catch((e: Error) => undefined)
  );
};

export const useCreateUserMutation = () => {
  const client = useQueryClient();
  return useMutation((data: UserData) => userApi.create(data), {
    onSuccess: async (result, variables, context) => {
      await client.invalidateQueries(["users", "list"]);
      await client.invalidateQueries([operatorsQueryKeys.root]);
    },
  });
};

export const useUpdateInfoUserMutation = () => {
  const client = useQueryClient();
  return useMutation(
    (params: { userId: User["id"]; info: UserInfo }) =>
      userApi.updateInfo(params.userId, params.info),
    {
      onSuccess: async (_, { info, userId }, context) => {
        await client.setQueryData<User[]>(["users", "list"], (prev) =>
          prev!.map((x) => (x.id === userId ? { ...x, info } : x))
        );
        await client.setQueryData<User | undefined>(
          ["users", "detail", userId],
          (prev) => (prev ? { ...prev, info } : prev)
        );
        await client.invalidateQueries([operatorsQueryKeys.root]);
      },
    }
  );
};

export const useRemoveUserMutation = () => {
  const client = useQueryClient();
  return useMutation(userApi.delete, {
    onSuccess: async (_, userId, context) => {
      await client.setQueryData<User[]>(
        ["users", "list"],
        (prev) => prev?.filter((x) => x.id !== userId) ?? []
      );
      await client.invalidateQueries(["users", "detail", userId]);
      await client.invalidateQueries([operatorsQueryKeys.root]);
    },
  });
};
