import { ComponentMeta } from '@storybook/react';
import {
  makeStoryFactory,
  StorybookWrapper,
} from '../../../../../libs/dev/storybook/src';
import { LayoutRoot } from '../../layout';
import React from 'react';
import { TicketIndex } from './ticket-index';
import { adminHelpdeskStubApi } from '../../../../../libs/entities/admin/stubs/src';
import { MswHandlers } from '../../../../../libs/dev/http-stubs/src';

export default {
  component: TicketIndex,
  title: 'views/TicketIndex',
} as ComponentMeta<typeof TicketIndex>;

const factory = makeStoryFactory(() => (
  <StorybookWrapper>
    <LayoutRoot>
      <TicketIndex />
    </LayoutRoot>
  </StorybookWrapper>
));

export const Primary = factory.create({
  parameters: {
    msw: {
      handlers: [
        adminHelpdeskStubApi
          .getTicketView({ ticketId: '*' as any })
          .handle(MswHandlers.success({})),
        adminHelpdeskStubApi
          .recreateTicketView({ ticketId: '*' as any })
          .handle(MswHandlers.success({})),
      ],
    },
  },
});
