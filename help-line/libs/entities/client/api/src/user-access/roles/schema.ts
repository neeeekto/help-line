import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { Project, Role, RoleData, User } from '@help-line/entities/client/api';

export const RolesClientApiSchema = {
  get: createApiAction<Role[], void>({
    method: HttpMethod.GET,
    url: '/v1/users-access/roles/',
  }),

  add: createApiAction<Role['id'], { data: RoleData }>({
    method: HttpMethod.POST,
    url: '/v1/users-access/roles/',
    data: ({ data }) => data,
  }),

  update: createApiAction<Role['id'], { roleId: Role['id']; data: RoleData }>({
    method: HttpMethod.PATCH,
    url: ({ roleId }) => `/v1/users-access/roles/${roleId}`,
    data: ({ data }) => data,
  }),

  delete: createApiAction<Role['id'], { roleId: Role['id'] }>({
    method: HttpMethod.DELETE,
    url: ({ roleId }) => `/v1/users-access/roles/${roleId}`,
  }),
};
