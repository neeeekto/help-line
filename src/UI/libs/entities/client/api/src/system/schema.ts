import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { AppState, Settings, SystemMessage, SystemMessageData } from './types';

export const SystemClientApiSchema = {
  getSettings: createApiAction<Settings>({
    method: HttpMethod.GET,
    url: '/v1/system/settings/',
  }),
  getState: createApiAction<AppState>({
    method: HttpMethod.GET,
    url: '/v1/system/state/',
  }),

  getMessages: createApiAction<SystemMessage[], { all?: boolean }>({
    method: HttpMethod.GET,
    url: '/v1/system/messages/',
    params: ({ all }) => ({ all: all || false }),
  }),

  addMessage: createApiAction<SystemMessage, { data?: SystemMessageData }>({
    method: HttpMethod.POST,
    url: '/v1/system/messages/',
    data: ({ data }) => data,
  }),
  updateMessage: createApiAction<
    SystemMessage,
    { id: SystemMessage['id']; data: SystemMessageData }
  >({
    method: HttpMethod.PATCH,
    url: ({ id }) => `/v1/system/messages/${id}`,
    data: ({ data }) => data,
  }),

  deleteMessage: createApiAction<SystemMessage, { id: SystemMessage['id'] }>({
    method: HttpMethod.DELETE,
    url: ({ id }) => `/v1/system/messages/${id}`,
  }),
};
