import { makeEventServiceHook } from "@core/events/events.hooks";
import { Ticket } from "@entities/helpdesk/tickets";
import { Guid } from "@entities/common";
import { OperatorView } from "@entities/helpdesk/operators";
import { Project } from "@entities/helpdesk/projects";

export const useTicketsEvents = makeEventServiceHook(
  "tickets",
  {
    OnUpdated: (ticketId: Ticket["id"], newEventsIds: Guid[]) => ({
      ticketId,
      newEventsIds,
    }),
    OnCreated: (ticketId: Ticket["id"]) => ({ ticketId }),
    OnOpen: (ticketId: Ticket["id"], operatorId: OperatorView["id"]) => ({
      ticketId,
      operatorId,
    }),
    OnClose: (ticketId: Ticket["id"], operatorId: OperatorView["id"]) => ({
      ticketId,
      operatorId,
    }),
  },
  {
    Subscribe: (projectId: Project["id"]) => [projectId],
    Unsubscribe: (projectId: Project["id"]) => [projectId],
  }
);

export const useTicketEvents = makeEventServiceHook(
  "ticket",
  {
    OnUpdated: (newEventsIds: Guid[]) => ({
      newEventsIds,
    }),
    OnOpen: (operatorId: OperatorView["id"]) => ({
      operatorId,
    }),
    OnClose: (operatorId: OperatorView["id"]) => ({
      operatorId,
    }),
  },
  {
    Subscribe: (ticketId: Ticket["id"]) => [ticketId],
    Unsubscribe: (ticketId: Ticket["id"]) => [ticketId],
    Open: (
      projectId: Project["id"],
      ticketId: Ticket["id"],
      operatorId: OperatorView["id"]
    ) => [projectId, ticketId, operatorId],
    Close: (
      projectId: Project["id"],
      ticketId: Ticket["id"],
      operatorId: OperatorView["id"]
    ) => [projectId, ticketId, operatorId],
  }
);

export const useTestEvents = makeEventServiceHook(
  "test",
  {
    onEvent: (newEventsIds: Guid[]) => ({
      newEventsIds,
    }),
  },
  {}
);

useTestEvents().add({
  onEvent: (data) => {
    console.log(data.newEventsIds);
  },
});
