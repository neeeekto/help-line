import { useQuery } from '@tanstack/react-query';
import {
  ProjectsClientApi,
  RemindersClientApi,
} from '@help-line/entities/client/api';
import { T_20_MIN, ROOT_QUERY_KEY } from '../constants';
import { useInjection } from 'inversify-react';

export const useProjectsQueries = () => {
  const api = useInjection(ProjectsClientApi);
  return useQuery([ROOT_QUERY_KEY, 'projects', 'list'], () => api.get(), {
    staleTime: T_20_MIN,
  });
};
