import { useMutation, useQuery, useQueryClient } from "react-query";
import { Ticket, TicketAction } from "./types";
import { ticketApi } from "./ticket.api";
import { Project } from "@entities/helpdesk/projects";

export const useTicketQuery = (
  ticketId: Ticket["id"],
  projectId: Project["id"]
) =>
  useQuery([projectId, "ticket", ticketId], () => ticketApi.getById(ticketId));
export const useTicketAtDateQuery = (
  ticketId: Ticket["id"],
  projectId: Project["id"],
  date: Date
) =>
  useQuery([projectId, "ticket", ticketId, date], (ctx) =>
    ticketApi.getByIdAtDate(ticketId, date)
  );

export const useTicketActionMutation = (
  ticketId: Ticket["id"],
  projectId: Project["id"]
) => {
  const client = useQueryClient();
  return useMutation(
    ["ticket", ticketId, "execute"],
    (actions: TicketAction[]) => ticketApi.execute(ticketId, actions),
    {
      onSuccess: (data) => {
        return client.invalidateQueries([projectId, "ticket", ticketId]);
      },
    }
  );
};

export const useTicketRetryMessageMutation = (
  ticketId: Ticket["id"],
  projectId: Project["id"],
  messageId: string,
  userId: string
) => {
  const client = useQueryClient();
  return useMutation(
    ["ticket", ticketId, "retry", messageId, userId],
    () => ticketApi.retryMessage(ticketId, messageId, userId),
    {
      onSuccess: (data) => {
        return client.invalidateQueries([projectId, "ticket", ticketId]);
      },
    }
  );
};
