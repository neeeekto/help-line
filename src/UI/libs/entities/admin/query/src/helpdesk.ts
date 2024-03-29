import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { createQueryKeys } from '@help-line/modules/query';
import {
  HelpdeskAdminApi,
  TicketSchedule,
  TicketScheduleStatus,
} from '@help-line/entities/admin/api';
import { Ticket } from '@help-line/entities/client/api';
import { useInjection } from 'inversify-react';

export const adminHelpdeskQueryKeys = createQueryKeys(
  ['api', 'admin', 'helpdesk'],
  ({ makeKey }) => ({
    schedules: createQueryKeys(makeKey('schedules'), ({ makeKey }) => ({
      byStatuses: (statuses: TicketScheduleStatus[]) => makeKey(statuses),
      byTicket: (ticketId: string) => makeKey('byTicket', ticketId),
    })),
  })
);

export const useSchedulesQuery = (statuses: TicketScheduleStatus[]) => {
  const api = useInjection(HelpdeskAdminApi);
  return useQuery(adminHelpdeskQueryKeys.schedules.byStatuses(statuses), () =>
    api.getSchedules({ statuses })
  );
};

export const useSchedulesByTicketQuery = (ticketId: Ticket['id']) => {
  const api = useInjection(HelpdeskAdminApi);
  return useQuery(
    adminHelpdeskQueryKeys.schedules.byTicket(ticketId),
    () => api.getSchedulesByTicket({ ticketId }),
    {
      refetchInterval: 1000 * 60,
      enabled: !!ticketId,
    }
  );
};

export const useReScheduleMutation = (scheduleId: TicketSchedule['id']) => {
  const client = useQueryClient();
  const api = useInjection(HelpdeskAdminApi);
  return useMutation(
    [...adminHelpdeskQueryKeys.root, 'schedules', 're', scheduleId],
    () => api.reschedule({ scheduleId }),
    {
      onSuccess: () => client.invalidateQueries(['hd', 'schedules', 'list']),
    }
  );
};

export const useDeleteScheduleMutation = (scheduleId: TicketSchedule['id']) => {
  const client = useQueryClient();
  const api = useInjection(HelpdeskAdminApi);
  return useMutation(
    [...adminHelpdeskQueryKeys.root, 'schedules', 'delete', scheduleId],
    () => api.delete({ scheduleId }),
    {
      onSuccess: () =>
        client.invalidateQueries(adminHelpdeskQueryKeys.schedules.root),
    }
  );
};
