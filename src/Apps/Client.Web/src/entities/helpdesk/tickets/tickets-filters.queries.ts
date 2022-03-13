import { useMutation, useQuery, useQueryClient } from "react-query";
import { Project } from "@entities/helpdesk/projects";
import { ticketsFilterApi } from "@entities/helpdesk/tickets/tickets-filters.api";
import { Filter } from "@entities/filter";
import {
  TicketFilter,
  TicketFilterData,
} from "@entities/helpdesk/tickets/types";

const namesKeyFactory = {
  root: "ticket-filters",
  list: (projectId: Project["id"]) => [projectId, namesKeyFactory.root, "list"],
  one: (projectId: Project["id"], filterId: TicketFilter["id"]) => [
    projectId,
    namesKeyFactory.root,
    "one",
    filterId,
  ],
};

export const useTicketsFiltersQuery = (
  projectId: Project["id"],
  features?: string[]
) =>
  useQuery([...namesKeyFactory.list(projectId), features], () =>
    ticketsFilterApi.get(features)
  );

export const useTicketsFilterQuery = (
  projectId: Project["id"],
  filterId: TicketFilter["id"]
) =>
  useQuery(namesKeyFactory.one(projectId, filterId), () =>
    ticketsFilterApi.getById(filterId)
  );

export const useCreateTicketFilterMutation = (projectId: Project["id"]) => {
  const client = useQueryClient();
  return useMutation(
    [projectId, namesKeyFactory.root, "create"],
    ticketsFilterApi.add,
    {
      onSuccess: (data, variables, context) =>
        client.invalidateQueries(namesKeyFactory.list(projectId)),
    }
  );
};

export const useUpdateTicketFilterMutation = (
  projectId: Project["id"],
  filterId: TicketFilter["id"]
) => {
  const client = useQueryClient();
  return useMutation(
    [projectId, namesKeyFactory.root, filterId, "update"],
    (data: TicketFilterData) => ticketsFilterApi.update(filterId, data),
    {
      onSuccess: (data, variables, context) =>
        Promise.all([
          client.invalidateQueries(namesKeyFactory.list(projectId)),
          client.invalidateQueries(namesKeyFactory.one(projectId, filterId)),
        ]),
    }
  );
};

export const useDeleteTicketFilterMutation = (
  projectId: Project["id"],
  filterId: TicketFilter["id"]
) => {
  const client = useQueryClient();
  return useMutation(
    [projectId, namesKeyFactory.root, filterId, "delete"],
    () => ticketsFilterApi.delete(filterId),
    {
      onSuccess: (data, variables, context) => {
        client.removeQueries(namesKeyFactory.one(projectId, filterId));
        return client.invalidateQueries(namesKeyFactory.list(projectId));
      },
    }
  );
};

export default namesKeyFactory;
