import { createStubFactory } from '@help-line/modules/http-stubs';
import {
  TicketSchedule,
  TicketScheduleStatus,
} from '@help-line/entities/admin/api';
import { faker } from '@faker-js/faker';

export namespace AdminHelpdeskStubs {
  export const createTicketSchedule = createStubFactory<TicketSchedule>(() => ({
    id: faker.datatype.uuid(),
    ticketId: `0-${faker.random.numeric(6)}`,
    triggerDate: faker.datatype.datetime().toISOString(),
    createdAt: faker.datatype.datetime().toISOString(),
    status: TicketScheduleStatus.Dead,
  }));
}
