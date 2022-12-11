import { makeRudSchema } from '../../api.presets';
import { Tag, TagDescriptionData, TagDescription } from './types';
import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { makeHeaderWithProject, ProjectApiRequest } from '../../schema.share';

export const TagsClientApiSchema = {
  ...makeRudSchema<Tag, boolean, Tag['key'], ProjectApiRequest>(
    '/v1/hd/tags',
    makeHeaderWithProject
  ),
  saveMany: createApiAction<
    void,
    { tags: string[]; enabled: boolean } & ProjectApiRequest
  >({
    method: HttpMethod.POST,
    header: makeHeaderWithProject,
    url: `/v1/hd/tags`,
    data: ({ tags, enabled }) => ({ tags, enabled }),
  }),
};

export const TagDescriptionsClientApiSchema = {
  ...makeRudSchema<
    TagDescription,
    TagDescriptionData,
    TagDescription['tag'],
    ProjectApiRequest
  >('/v1/hd/tags/description', makeHeaderWithProject),
};
