import { makeRudSchema } from '../../api.presets';
import { AutoReply, AutoReplyData } from './types';
import { ProjectApiRequest, makeHeaderWithProject } from '../../schema.share';

export const AutoRepliesClientApiSchema = makeRudSchema<
  AutoReply,
  AutoReplyData,
  AutoReply['id'],
  ProjectApiRequest
>('/v1/hd/automations/replies', makeHeaderWithProject);
