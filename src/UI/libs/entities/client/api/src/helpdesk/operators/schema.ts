import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { Operator, OperatorRole, OperatorRoleData } from './types';
import {
  makeHeaderWithProject,
  ProjectApiRequest,
  Ticket,
} from '@help-line/entities/client/api';
import { makeCrudSchema } from '../../api.presets';

export const OperatorsClientApiSchema = {
  get: createApiAction<Operator[], ProjectApiRequest>({
    method: HttpMethod.GET,
    url: '/v1/hd/operators/',
    header: makeHeaderWithProject,
  }),

  getOne: createApiAction<
    Operator,
    ProjectApiRequest & { operatorId: Operator['id'] }
  >({
    method: HttpMethod.GET,
    url: ({ operatorId }) => `/v1/hd/operators/${operatorId}`,
    header: makeHeaderWithProject,
  }),

  setRoles: createApiAction<
    Operator,
    { operatorId: Operator['id']; rolesIds: string[] } & ProjectApiRequest
  >({
    method: HttpMethod.PATCH,
    url: ({ operatorId }) => `/v1/hd/operators/${operatorId}/roles`,
    header: makeHeaderWithProject,
    data: ({ rolesIds }) => rolesIds,
  }),

  addFavorite: createApiAction<
    void,
    { ticketId: Ticket['id']; operatorId: Operator['id'] }
  >({
    method: HttpMethod.POST,
    url: ({ ticketId, operatorId }) =>
      `/v1/hd/operators/${operatorId}/favorite/${ticketId}`,
  }),
  removeFavorite: createApiAction<
    void,
    { ticketId: Ticket['id']; operatorId: Operator['id'] }
  >({
    method: HttpMethod.DELETE,
    url: ({ ticketId, operatorId }) =>
      `/v1/hd/operators/${operatorId}/favorite/${ticketId}`,
  }),
};

export const OperatorsRolesClientApiSchema = {
  ...makeCrudSchema<
    OperatorRole,
    OperatorRoleData,
    OperatorRoleData,
    OperatorRole['id'],
    ProjectApiRequest
  >('/v1/hd/operators/roles', makeHeaderWithProject),
  getOne: createApiAction<OperatorRole, { roleId: OperatorRole['id'] }>({
    method: HttpMethod.GET,
    url: ({ roleId }) => `/v1/hd/operators/roles/${roleId}/`,
  }),
};
