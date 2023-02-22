import {
  TicketAction,
  TicketClientApi,
  TicketId,
} from '@help-line/entities/client/api';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { createQueryKeys } from '@help-line/modules/query';
import { ROOT_QUERY_KEY } from '../constants';
import { useInjection } from 'inversify-react';

export const clientTicketQueryKeys = createQueryKeys(
  [ROOT_QUERY_KEY, 'ticket'],
  ({ makeKey }) => ({
    get: (ticketId: TicketId) => makeKey(ticketId),
    atCurrent: (ticketId: TicketId) => makeKey(ticketId, 'current'),
    atDate: (ticketId: TicketId, date: Date) =>
      makeKey(ticketId, date.toDateString()),
  })
);

export const useTicketQuery = (ticketId: TicketId) => {
  const api = useInjection(TicketClientApi);
  return useQuery(
    clientTicketQueryKeys.atCurrent(ticketId),
    () => api.getById({ ticketId }),
    {
      enabled: !!ticketId,
    }
  );
};

export const useTicketAdDateQuery = (ticketId: TicketId, date: Date) => {
  const api = useInjection(TicketClientApi);
  return useQuery(clientTicketQueryKeys.atDate(ticketId, date), () =>
    api.getByIdAtDate({ ticketId, date })
  );
};

export const useExecuteTicketMutation = (
  ticketId: TicketId,
  version: number
) => {
  const api = useInjection(TicketClientApi);
  const qClient = useQueryClient();
  return useMutation(
    [...clientTicketQueryKeys.root, ticketId, 'execute'],
    (actions: TicketAction[]) => api.execute({ ticketId, version, actions }),
    {
      onSuccess: () => {
        return qClient.invalidateQueries(clientTicketQueryKeys.get(ticketId));
      },
    }
  );
};
