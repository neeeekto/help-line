import { useQueries, useQuery } from "react-query";
import { ticketMetaApi } from "./ticket-meta.api";

export const useTicketSearchModelQuery = () =>
  useQuery(["tickets-meta", "search"], ticketMetaApi.getSearchModel, {
    cacheTime: Number.POSITIVE_INFINITY,
    staleTime: Number.POSITIVE_INFINITY,
  });

export const useTicketCtxModelQuery = () =>
  useQuery(["tickets-meta", "context"], ticketMetaApi.getCtxModel, {
    cacheTime: Number.POSITIVE_INFINITY,
    staleTime: Number.POSITIVE_INFINITY,
  });
