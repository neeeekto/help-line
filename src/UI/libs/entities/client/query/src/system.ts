import { createQueryKeys } from '@help-line/modules/query';
import { ROOT_QUERY_KEY } from './constants';
import { useApi } from '@help-line/modules/api';
import { SystemClientApi } from '@help-line/entities/client/api';
import { useQuery } from '@tanstack/react-query';

export const clientSystemQueryKeys = createQueryKeys(
  [ROOT_QUERY_KEY, 'system'],
  ({ makeKey }) => ({
    messages: () => makeKey('messages'),
  })
);

export const useSystemMessagesQuery = (all = false) => {
  const api = useApi(SystemClientApi);
  return useQuery(clientSystemQueryKeys.messages(), () =>
    api.getMessages({ all })
  );
};

export const useSystemSettingsQuery = () => {
  const api = useApi(SystemClientApi);
  return useQuery(clientSystemQueryKeys.messages(), () => api.getSettings());
};

export const useSystemStateQuery = () => {
  const api = useApi(SystemClientApi);
  return useQuery(clientSystemQueryKeys.messages(), () => api.getState());
};
