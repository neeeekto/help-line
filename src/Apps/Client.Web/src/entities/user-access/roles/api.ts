import { httpClient, HttpClient } from "@core/http";
import { Role, RoleData } from "./types";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeRolesApi = (http: HttpClient) => ({
  get: () => http.get<Role[]>("/api/v1/user-access/roles").then((x) => x.data),
  add: (data: RoleData) =>
    http.post<string>("/api/v1/user-access/roles", data).then((x) => x.data),
  update: (roleId: Role["id"], data: RoleData) =>
    http
      .patch<void>(`/api/v1/user-access/roles/${roleId}`, data)
      .then((x) => x.data),
  delete: (roleId: Role["id"]) =>
    http
      .delete<void>(`/api/v1/user-access/roles/${roleId}`)
      .then((x) => x.data),
});

export const rolesApi = makeRolesApi(httpClient);
