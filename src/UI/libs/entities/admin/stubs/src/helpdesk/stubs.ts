import { createStubFactory } from '@help-line/dev/http-stubs';
import {
  TicketSchedule,
  TicketScheduleStatus,
} from '@help-line/entities/admin/api';
import { faker } from '@faker-js/faker';

export namespace AdminHelpdeskStubs {
  export const createTicketId = () => `0-${faker.random.numeric(6)}`;
  export const createTicketSchedule = createStubFactory<TicketSchedule>(() => ({
    id: faker.datatype.uuid(),
    ticketId: createTicketId(),
    triggerDate: faker.datatype.datetime().toISOString(),
    createdAt: faker.datatype.datetime().toISOString(),
    status: TicketScheduleStatus.Dead,
  }));
}
