import { AutoRepliesClientApi } from '@help-line/entities/client/api';
import { makeQueryAndMutationForRudApi } from '../utils';

export const {
  clientAutoReplyQueryKeys,
  useAutoReplyListQuery,
  useDeleteAutoReplyMutation,
  useSaveAutoReplyMutation,
} = makeQueryAndMutationForRudApi(
  'AutoReply',
  ['automations', 'reply'],
  AutoRepliesClientApi,
  (args) => [args.projectId]
);
