import { httpClient, HttpClient, makeCrudApi } from "@core/http";
import {
  OperatorView,
  Operator,
  OperatorRole,
  OperatorRoleData,
} from "./types";
import {makeTicketsFilterApi, Ticket} from "@entities/helpdesk/tickets";
import {makeUseHookForApi} from "@core/http/api.hooks";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeOperatorApi = (http: HttpClient) => ({
  getListView: () =>
    http.get<OperatorView[]>(`/api/v1/hd/operators/simple`).then((x) => x.data),
  getOneView: (operatorId: string) =>
    http
      .get<OperatorView>(`/api/v1/hd/operators/${operatorId}`)
      .then((x) => x.data),
  getList: () =>
    http.get<Operator[]>(`/api/v1/hd/operators`).then((x) => x.data),
  getOne: (operatorId: Operator["id"]) =>
    http
      .get<Operator>(`/api/v1/hd/operators/${operatorId}`)
      .then((x) => x.data),
  getMe: () =>
    http.get<Operator>(`/api/v1/hd/operators/me`).then((x) => x.data),
  addFavorite: (ticketId: Ticket["id"]) =>
    http
      .post<void>(`/api/v1/hd/operators/favorite/${ticketId}`)
      .then((x) => x.data),
  removeFavorite: (ticketId: Ticket["id"]) =>
    http
      .delete<void>(`/api/v1/hd/operators/favorite/${ticketId}`)
      .then((x) => x.data),
  setOperatorRoles: (operatorId: Operator["id"], rolesIds: string[]) =>
    http
      .patch<Operator>(`/api/v1/hd/operators/${operatorId}/roles`, rolesIds)
      .then((x) => x.data),
});

export const makeRoleApi = (http: HttpClient) => ({
  ...makeCrudApi<OperatorRole, OperatorRoleData>(
    http,
    "/api/v1/hd/operators/roles"
  ),
  getOne: (roleId: string) =>
    http
      .get<OperatorRole>(`/api/v1/hd/operators/roles/${roleId}`)
      .then((x) => x.data),
});

export const useOperatorApi = makeUseHookForApi(makeOperatorApi);
export const useOperatorRoleApi = makeUseHookForApi(makeRoleApi);
