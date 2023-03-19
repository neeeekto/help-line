import { ComponentMeta } from '@storybook/react';
import { makeStoryFactory } from '../../../../../libs/dev/storybook/src';
import React, { ComponentProps } from 'react';
import {
  adminHelpdeskStubApi,
  AdminHelpdeskStubs,
  adminProjectsStubApi,
  AdminProjectStubs,
} from '../../../../../libs/entities/admin/stubs/src';
import { MswHandlers } from '../../../../../libs/dev/http-stubs/src';
import { CreateTicketView } from './create-ticket-view';
import { LayoutRoot } from '../../layout';
export default {
  component: CreateTicketView,
  title: 'views/CreateTicketView',
} as ComponentMeta<typeof CreateTicketView>;

const factory = makeStoryFactory(
  (args: ComponentProps<typeof CreateTicketView>) => (
    <LayoutRoot>
      <CreateTicketView {...args} />
    </LayoutRoot>
  )
);

export const Primary = factory.create({
  parameters: {
    msw: {
      handlers: [
        adminProjectsStubApi
          .get()
          .handle(
            MswHandlers.success([
              AdminProjectStubs.createProject({}),
              AdminProjectStubs.createProject({}),
            ])
          ),
        adminHelpdeskStubApi.createTicket({}).handle(
          MswHandlers.makeHandler((req, res, context) => {
            return res(
              context.delay(500),
              context.status(200),
              context.json(AdminHelpdeskStubs.createTicketId())
            );
          })
        ),
      ],
    },
  },
});

export const Loading = factory.create({
  parameters: {
    msw: {
      handlers: [adminProjectsStubApi.get().handle(MswHandlers.delay(100000))],
    },
  },
});

export const Error = factory.create({
  parameters: {
    msw: {
      handlers: [adminProjectsStubApi.get().handle(MswHandlers.error(500))],
    },
  },
});

export const WithCreatedTicket = factory.create({
  args: {
    lastCreatedTicket: AdminHelpdeskStubs.createTicketId(),
  },
  parameters: {},
});
