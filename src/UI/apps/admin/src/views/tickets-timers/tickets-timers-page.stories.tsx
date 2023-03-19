import { ComponentMeta } from '@storybook/react';
import { makeStoryFactory } from '../../../../../libs/dev/storybook/src';
import React from 'react';
import { LayoutRoot } from '../../layout';
import {
  adminHelpdeskStubApi,
  AdminHelpdeskStubs,
} from '../../../../../libs/entities/admin/stubs/src';
import { MswHandlers } from '../../../../../libs/dev/http-stubs/src';
import { TicketTimersPage } from './tickets-timers-page';
import { TicketScheduleStatus } from '../../../../../libs/entities/admin/api/src';

export default {
  component: TicketTimersPage,
  title: 'views/TicketTimersPage',
} as ComponentMeta<typeof TicketTimersPage>;

const factory = makeStoryFactory(() => (
  <LayoutRoot>
    <TicketTimersPage />
  </LayoutRoot>
));

const TICKET_ID = AdminHelpdeskStubs.createTicketId();

export const Primary = factory.create({
  parameters: {
    msw: {
      handlers: [
        adminHelpdeskStubApi
          .getSchedulesByTicket({ ticketId: '*' as any })
          .handle(
            MswHandlers.success([
              AdminHelpdeskStubs.createTicketSchedule({
                ticketId: TICKET_ID,
                status: TicketScheduleStatus.Dead,
              }),
              AdminHelpdeskStubs.createTicketSchedule({
                ticketId: TICKET_ID,
                status: TicketScheduleStatus.InQueue,
              }),
            ])
          ),
        adminHelpdeskStubApi.getSchedules({}).handle(
          MswHandlers.success([
            AdminHelpdeskStubs.createTicketSchedule({
              status: TicketScheduleStatus.Dead,
            }),
            AdminHelpdeskStubs.createTicketSchedule({
              status: TicketScheduleStatus.Dead,
            }),
            AdminHelpdeskStubs.createTicketSchedule({
              status: TicketScheduleStatus.Planned,
            }),
            AdminHelpdeskStubs.createTicketSchedule({
              status: TicketScheduleStatus.Planned,
            }),
            AdminHelpdeskStubs.createTicketSchedule({
              status: TicketScheduleStatus.Problem,
            }),
            AdminHelpdeskStubs.createTicketSchedule({
              status: TicketScheduleStatus.Problem,
            }),
            AdminHelpdeskStubs.createTicketSchedule({
              status: TicketScheduleStatus.InQueue,
            }),
            AdminHelpdeskStubs.createTicketSchedule({
              status: TicketScheduleStatus.InQueue,
            }),
          ])
        ),
      ],
    },
  },
});

export const Loading = factory.create({
  parameters: {
    msw: {
      handlers: [
        adminHelpdeskStubApi
          .getSchedules({})
          .handle(MswHandlers.delay('infinite')),
      ],
    },
  },
});
