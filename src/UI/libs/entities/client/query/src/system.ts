import { createQueryKeys } from '@help-line/modules/query';
import { ROOT_QUERY_KEY } from './constants';
import { SystemClientApi } from '@help-line/entities/client/api';
import { useQuery } from '@tanstack/react-query';
import { useInjection } from 'inversify-react';

export const clientSystemQueryKeys = createQueryKeys(
  [ROOT_QUERY_KEY, 'system'],
  ({ makeKey }) => ({
    messages: () => makeKey('messages'),
  })
);

export const useSystemMessagesQuery = (all = false) => {
  const api = useInjection(SystemClientApi);
  return useQuery(clientSystemQueryKeys.messages(), () =>
    api.getMessages({ all })
  );
};

export const useSystemSettingsQuery = () => {
  const api = useInjection(SystemClientApi);
  return useQuery(clientSystemQueryKeys.messages(), () => api.getSettings());
};

export const useSystemStateQuery = () => {
  const api = useInjection(SystemClientApi);
  return useQuery(clientSystemQueryKeys.messages(), () => api.getState());
};
