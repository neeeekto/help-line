import { makeQueryAndMutationForCrudApi } from '../utils';
import { RemindersClientApi } from '@help-line/entities/client/api';

export const {
  useDeleteReminderMutation,
  useCreateReminderMutation,
  useUpdateReminderMutation,
  clientReminderQueryKeys,
  useReminderListQuery,
} = makeQueryAndMutationForCrudApi(
  'Reminder',
  ['reminder'],
  RemindersClientApi,
  (args) => [args.projectId]
);
