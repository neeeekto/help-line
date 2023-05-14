import { ComponentProps } from 'react';
import { Meta, StoryObj } from '@storybook/react';

import { TicketQueryFilterEditor } from './ticket-query-filter-editor';

type SBComponentProps = ComponentProps<typeof TicketQueryFilterEditor>;
type Story = StoryObj<SBComponentProps>;

export default {
  title: 'TicketQueryFilterEditor',
  component: TicketQueryFilterEditor,
  args: {},
  render: (args, context) => {
    return (
      <div style={{ height: 500 }}>
        <TicketQueryFilterEditor />
      </div>
    );
  },
} as Meta<ComponentProps<typeof TicketQueryFilterEditor>>;

export const Primary: Story = {};
