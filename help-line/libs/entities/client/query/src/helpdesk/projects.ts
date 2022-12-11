import { useQuery } from '@tanstack/react-query';
import { useApi } from '@help-line/modules/api';
import {
  ProjectsClientApi,
  RemindersClientApi,
} from '@help-line/entities/client/api';
import { T_20_MIN, ROOT_QUERY_KEY } from '../constants';

export const useProjectsQueries = () => {
  const api = useApi(ProjectsClientApi);
  return useQuery([ROOT_QUERY_KEY, 'projects', 'list'], () => api.get(), {
    staleTime: T_20_MIN,
  });
};
