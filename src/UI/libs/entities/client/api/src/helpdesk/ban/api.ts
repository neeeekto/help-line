import { createApiClassBySchema } from '@help-line/modules/http';
import { BanClientApiSchema } from './schema';

export class BanClientApi extends createApiClassBySchema(BanClientApiSchema) {}
