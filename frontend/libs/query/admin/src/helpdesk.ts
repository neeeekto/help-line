import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  HelpdeskAdminApi,
  TicketSchedule,
  TicketScheduleStatus,
} from '@help-line/api/admin';
import { useApiClient } from '@help-line/modules/api';

export const adminHelpdeskQueryKeys = {
  root: ['admin', 'helpdesk'] as const,
  schedulesRoot: () => [...adminHelpdeskQueryKeys.root, 'schedules'] as const,
  schedules: (statuses: TicketScheduleStatus[]) =>
    [...adminHelpdeskQueryKeys.schedulesRoot(), statuses] as const,
  schedulesByTicket: (ticketId: string) =>
    [...adminHelpdeskQueryKeys.schedulesRoot(), 'byTicket', ticketId] as const,
};

export const useSchedulesQuery = (statuses: TicketScheduleStatus[]) => {
  const api = useApiClient(HelpdeskAdminApi);
  return useQuery(adminHelpdeskQueryKeys.schedules(statuses), () =>
    api.getSchedules(statuses)
  );
};

export const useSchedulesByTicketQuery = (ticketId: string) => {
  const api = useApiClient(HelpdeskAdminApi);
  return useQuery(
    adminHelpdeskQueryKeys.schedulesByTicket(ticketId),
    () => api.getSchedulesByTicket(ticketId),
    {
      refetchInterval: 1000 * 60,
    }
  );
};

export const useReScheduleMutation = (scheduleId: TicketSchedule['id']) => {
  const client = useQueryClient();
  const api = useApiClient(HelpdeskAdminApi);
  return useMutation(
    [...adminHelpdeskQueryKeys.root, 'schedules', 're', scheduleId],
    () => api.reschedule(scheduleId),
    {
      onSuccess: () => client.invalidateQueries(['hd', 'schedules', 'list']),
    }
  );
};

export const useDeleteScheduleMutation = (scheduleId: TicketSchedule['id']) => {
  const client = useQueryClient();
  const api = useApiClient(HelpdeskAdminApi);
  return useMutation(
    [...adminHelpdeskQueryKeys.root, 'schedules', 'delete', scheduleId],
    () => api.delete(scheduleId),
    {
      onSuccess: () =>
        client.invalidateQueries(adminHelpdeskQueryKeys.schedulesRoot()),
    }
  );
};
