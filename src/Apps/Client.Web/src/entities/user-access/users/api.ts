import { httpClient, HttpClient } from "@core/http";
import { User, UserData, UserInfo, UserRoles } from "./types";
import { Project } from "@entities/helpdesk/projects";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeUserApi = (http: HttpClient) => ({
  get: (projectId?: Project["id"]) =>
    http
      .get<User[]>("/api/v1/users-access/users", { params: { projectId } })
      .then((x) => x.data),
  getById: (userId: User["id"]) =>
    http.get<User>(`/api/v1/users-access/users/${userId}`).then((x) => x.data),
  create: (data: UserData) =>
    http.post<string>(`/api/v1/users-access/users/`, data).then((x) => x.data),
  delete: (userId: User["id"]) =>
    http
      .delete<void>(`/api/v1/users-access/users/${userId}`)
      .then((x) => x.data),
  updateInfo: (userId: User["id"], info: UserInfo) =>
    http
      .patch<void>(`/api/v1/users-access/users/${userId}/info`, info)
      .then((x) => x.data),
  updatePermissions: (
    userId: User["id"],
    permissions: Array<UserData["permissions"]>
  ) =>
    http
      .patch<void>(
        `/api/v1/users-access/users/${userId}/permissions`,
        permissions
      )
      .then((x) => x.data),
  updateRoles: (userId: User["id"], roles: UserRoles) =>
    http
      .patch<void>(`/api/v1/users-access/users/${userId}/roles`, roles)
      .then((x) => x.data),
});

export const userApi = makeUserApi(httpClient);
