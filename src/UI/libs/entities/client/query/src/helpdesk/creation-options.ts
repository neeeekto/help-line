import { makeQueryAndMutationForRudApi } from '../utils';
import {
  CreationOptionsPlatformClientApi,
  CreationOptionsProblemAndThemesClientApi,
} from '@help-line/entities/client/api';

export const {
  useDeleteCreationOptionsPlatformMutation,
  useSaveCreationOptionsPlatformMutation,
  useCreationOptionsPlatformListQuery,
  clientCreationOptionsPlatformQueryKeys,
} = makeQueryAndMutationForRudApi(
  'CreationOptionsPlatform',
  ['create-options', 'platform'],
  CreationOptionsPlatformClientApi,
  (args) => [args.projectId]
);

export const {
  useDeleteCreationProblemAndThemesMutation,
  useCreationProblemAndThemesListQuery,
  useSaveCreationProblemAndThemesMutation,
  clientCreationProblemAndThemesQueryKeys,
} = makeQueryAndMutationForRudApi(
  'CreationProblemAndThemes',
  ['create-options', 'problem-and-themes'],
  CreationOptionsProblemAndThemesClientApi,
  (args) => [args.projectId]
);
