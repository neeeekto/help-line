import { useMutation, useQuery, useQueryClient } from "react-query";
import { Project } from "@entities/helpdesk/projects";
import { useTicketsFilterApi } from "@entities/helpdesk/tickets/tickets-filters.api";
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
) => {
  const api = useTicketsFilterApi();
  return useQuery([...namesKeyFactory.list(projectId), features], () =>
    api.get(features)
  );
};

export const useTicketsFilterQuery = (
  projectId: Project["id"],
  filterId: TicketFilter["id"]
) => {
  const api = useTicketsFilterApi();
  return useQuery(namesKeyFactory.one(projectId, filterId), () =>
    api.getById(filterId)
  );
};

export const useCreateTicketFilterMutation = (projectId: Project["id"]) => {
  const api = useTicketsFilterApi();
  const client = useQueryClient();
  return useMutation([projectId, namesKeyFactory.root, "create"], api.add, {
    onSuccess: (data, variables, context) =>
      client.invalidateQueries(namesKeyFactory.list(projectId)),
  });
};

export const useUpdateTicketFilterMutation = (
  projectId: Project["id"],
  filterId: TicketFilter["id"]
) => {
  const client = useQueryClient();
  const api = useTicketsFilterApi();
  return useMutation(
    [projectId, namesKeyFactory.root, filterId, "update"],
    (data: TicketFilterData) => api.update(filterId, data),
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
  const api = useTicketsFilterApi();
  return useMutation(
    [projectId, namesKeyFactory.root, filterId, "delete"],
    () => api.delete(filterId),
    {
      onSuccess: (data, variables, context) => {
        client.removeQueries(namesKeyFactory.one(projectId, filterId));
        return client.invalidateQueries(namesKeyFactory.list(projectId));
      },
    }
  );
};

export default namesKeyFactory;
