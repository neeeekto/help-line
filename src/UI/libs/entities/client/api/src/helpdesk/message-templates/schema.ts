import { makeCrudSchema, makeRudSchema } from '../../api.presets';
import { MessageTemplate, MessageTemplateData } from './types';
import { createApiAction, HttpMethod } from '@help-line/modules/http';
import { makeHeaderWithProject, ProjectApiRequest } from '../../schema.share';

export const MessageTemplateClientApiSchema = {
  ...makeCrudSchema<
    MessageTemplate,
    MessageTemplateData,
    MessageTemplateData,
    MessageTemplate['id'],
    ProjectApiRequest
  >('/v1/hd/message-templates', makeHeaderWithProject),
  changeOrder: createApiAction<
    void,
    { templateId: MessageTemplate['id']; order: number } & ProjectApiRequest
  >({
    method: HttpMethod.PATCH,
    url: ({ templateId }) => `/v1/hd/message-templates/${templateId}/order`,
    data: ({ order }) => order,
    header: makeHeaderWithProject,
  }),
};
