import { createApiAction, HttpMethod } from '@help-line/modules/http';
import {
  CreateTicketData,
  Recipient,
  Ticket,
  TicketAction,
  TicketFilter,
  TicketFilterData,
  TicketFilterValue,
  TicketIncomingMessageEvent,
  TicketOutgoingMessageEvent,
  TicketSortValue,
  TicketsSettings,
} from './types';
import { makeCrudSchema } from '../../api.presets';
import { makeHeaderWithProject, ProjectApiRequest } from '../../schema.share';

export const TicketsClientApiSchema = {
  create: createApiAction<
    Ticket['id'],
    { data: CreateTicketData } & ProjectApiRequest
  >({
    method: HttpMethod.POST,
    header: makeHeaderWithProject,
    url: `/v1/hd/tickets`,
    data: ({ data }) => data,
  }),

  search: createApiAction<
    Ticket['id'],
    {
      page: number;
      perPage: number;
      filter: TicketFilterValue;
      sorts: TicketSortValue[];
    } & ProjectApiRequest
  >({
    method: HttpMethod.POST,
    header: makeHeaderWithProject,
    url: `/v1/hd/tickets`,
    data: ({ filter, sorts }) => ({ filter, sorts }),
    params: ({ page, perPage }) => ({ page, perPage }),
  }),
};

export const TicketClientApiSchema = {
  getById: createApiAction<Ticket, { ticketId: Ticket['id'] }>({
    method: HttpMethod.GET,
    url: ({ ticketId }) => `/api/v1/hd/ticket/${ticketId}`,
  }),
  getByIdAtDate: createApiAction<
    Ticket,
    { ticketId: Ticket['id']; date: Date }
  >({
    method: HttpMethod.GET,
    url: ({ ticketId, date }) => `/v1/hd/ticket/${ticketId}/${date}`,
  }),
  execute: createApiAction<
    any[],
    {
      ticketId: Ticket['id'];
      actions: TicketAction[];
      version: Ticket['version'];
    }
  >({
    method: HttpMethod.POST,
    url: ({ ticketId }) => `/v1/hd/ticket/${ticketId}`,
    data: ({ actions }) => actions,
    header: ({ version }) => ({ ETag: version }),
  }),
  retryMessage: createApiAction<
    void,
    {
      ticketId: Ticket['id'];
      messageId: TicketOutgoingMessageEvent['messageId'];
      userId: Recipient['userId'];
    }
  >({
    method: HttpMethod.POST,
    url: ({ ticketId, userId, messageId }) =>
      `v1/hd/ticket/${ticketId}/messages/${userId}/${messageId}/retry`,
  }),
};

export const TicketsSettingClientApiSchema = {
  get: createApiAction<TicketsSettings, ProjectApiRequest>({
    method: HttpMethod.GET,
    header: makeHeaderWithProject,
    url: `/v1/hd/tickets-settings`,
  }),
  update: createApiAction<void, { data: TicketsSettings } & ProjectApiRequest>({
    method: HttpMethod.PATCH,
    header: makeHeaderWithProject,
    url: `/v1/hd/tickets-settings`,
    data: ({ data }) => data,
  }),
};

export const TicketsFiltersClientApiSchema = {
  ...makeCrudSchema<
    TicketFilter,
    TicketFilterData,
    TicketFilterData,
    TicketFilter['id'],
    ProjectApiRequest
  >('/v1/hd/tickets/search/filters', makeHeaderWithProject),
  get: createApiAction<
    TicketFilter[],
    ProjectApiRequest & { features?: string[] }
  >({
    method: HttpMethod.GET,
    header: makeHeaderWithProject,
    url: `/v1/hd/tickets/search/filters`,
    params: ({ features }) => ({
      features,
    }),
  }),
  getById: createApiAction<
    TicketFilter,
    ProjectApiRequest & { filterId: string }
  >({
    method: HttpMethod.GET,
    header: makeHeaderWithProject,
    url: ({ filterId }) => `/api/v1/hd/tickets/search/filters/${filterId}`,
  }),
};
