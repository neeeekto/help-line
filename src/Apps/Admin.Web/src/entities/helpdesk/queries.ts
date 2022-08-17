import { useMutation, useQuery, useQueryClient } from "react-query";
import { helpdeskApi } from "@entities/helpdesk/api";
import { TicketSchedule, TicketScheduleStatus } from "@entities/helpdesk/types";

export const useSchedulesQuery = (statuses: TicketScheduleStatus[]) =>
  useQuery(["hd", "schedules", "list", statuses], () =>
    helpdeskApi.getSchedules(statuses)
  );

export const useSchedulesByTicketQuery = (ticketId: string) =>
  useQuery(
    ["hd", "schedules", "byTicket", ticketId],
    () => helpdeskApi.getSchedulesByTicket(ticketId),
    {
      refetchInterval: 1000 * 60,
    }
  );

export const useReScheduleMutation = (scheduleId: TicketSchedule["id"]) => {
  const client = useQueryClient();
  return useMutation(
    ["hd", "schedules", "re", scheduleId],
    () => helpdeskApi.reschedule(scheduleId),
    {
      onSuccess: () => client.invalidateQueries(["hd", "schedules", "list"]),
    }
  );
};

export const useDeleteScheduleMutation = (scheduleId: TicketSchedule["id"]) => {
  const client = useQueryClient();
  return useMutation(
    ["hd", "schedules", "delete", scheduleId],
    () => helpdeskApi.delete(scheduleId),
    {
      onSuccess: () => client.invalidateQueries(["hd", "schedules", "list"]),
    }
  );
};
