import {
  CreationOptionsPlatformClientApiSchema,
  CreationOptionsProblemAndThemesClientApiSchema,
} from './schema';
import { createApiClassBySchema } from '@help-line/modules/http';

export class CreationOptionsPlatformClientApi extends createApiClassBySchema(
  CreationOptionsPlatformClientApiSchema
) {}

export class CreationOptionsProblemAndThemesClientApi extends createApiClassBySchema(
  CreationOptionsProblemAndThemesClientApiSchema
) {}
