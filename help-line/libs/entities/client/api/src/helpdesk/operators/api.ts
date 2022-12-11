import { createApiClassBySchema } from '@help-line/modules/http';
import {
  OperatorsClientApiSchema,
  OperatorsRolesClientApiSchema,
} from './schema';

export class OperatorsClientApi extends createApiClassBySchema(
  OperatorsClientApiSchema
) {}
export class OperatorsRolesClientApi extends createApiClassBySchema(
  OperatorsRolesClientApiSchema
) {}
