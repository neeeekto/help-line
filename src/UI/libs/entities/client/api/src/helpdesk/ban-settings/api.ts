/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
import { createApiClassBySchema } from '@help-line/modules/http';
import { BanSettingsClientApiSchema } from './schema';

export class BanSettingsClientApi extends createApiClassBySchema(
  BanSettingsClientApiSchema
) {}
