import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { User, UserData, UserInfo, UserRoles } from './types';
import { Project } from '../../helpdesk/projects';

export const UsersClientApiSchema = {
  get: createApiAction<User[], { projectId?: Project['id'] }>({
    method: HttpMethod.GET,
    url: '/v1/users-access/users/',
    params: ({ projectId }) => ({ projectId }),
  }),

  getById: createApiAction<User, { userId: User['id'] }>({
    method: HttpMethod.GET,
    url: ({ userId }) => `/v1/users-access/users/${userId}`,
  }),

  create: createApiAction<User['id'], { data: UserData }>({
    method: HttpMethod.POST,
    url: `/v1/users-access/users/`,
    data: ({ data }) => data,
  }),

  delete: createApiAction<void, { userId: User['id'] }>({
    method: HttpMethod.DELETE,
    url: ({ userId }) => `/v1/users-access/users/${userId}`,
  }),

  updateInfo: createApiAction<void, { userId: User['id']; info: UserInfo }>({
    method: HttpMethod.PATCH,
    url: ({ userId }) => `/v1/users-access/users/${userId}/info`,
    data: ({ info }) => info,
  }),

  updatePermissions: createApiAction<
    void,
    { userId: User['id']; permissions: UserData['permissions'] }
  >({
    method: HttpMethod.PATCH,
    url: ({ userId }) => `/v1/users-access/users/${userId}/permissions`,
    data: ({ permissions }) => permissions,
  }),

  updateRoles: createApiAction<void, { userId: User['id']; roles: UserRoles }>({
    method: HttpMethod.PATCH,
    url: ({ userId }) => `/v1/users-access/users/${userId}/roles`,
    data: ({ roles }) => roles,
  }),
};
