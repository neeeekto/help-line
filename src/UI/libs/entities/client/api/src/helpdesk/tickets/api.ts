import { createApiClassBySchema } from '@help-line/modules/http';
import {
  TicketClientApiSchema,
  TicketsClientApiSchema,
  TicketsFiltersClientApiSchema,
  TicketsSettingClientApiSchema,
} from './schema';

export class TicketsSettingClientApi extends createApiClassBySchema(
  TicketsSettingClientApiSchema
) {}

export class TicketsFiltersClientApi extends createApiClassBySchema(
  TicketsFiltersClientApiSchema
) {}

export class TicketsClientApi extends createApiClassBySchema(
  TicketsClientApiSchema
) {}

export class TicketClientApi extends createApiClassBySchema(
  TicketClientApiSchema
) {}
