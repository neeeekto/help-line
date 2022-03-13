import React from "react";
import { ComponentStory, ComponentMeta } from "@storybook/react";

import { TicketAssignAside } from "../assign.aside";
import { Ticket } from "../../../index";
import {AuthProvider} from "@core/auth";

const AsideStory: React.FC<{ readonly: boolean }> = ({ readonly }) => (
    <AuthProvider>
      <Ticket ticket={{} as any} readonly={readonly}>
        <Ticket.Aside>
          <Ticket.Aside.Assign />
        </Ticket.Aside>
      </Ticket>
    </AuthProvider>

);

export default {
  title: "Ticket/Aside/Assign",
  component: AsideStory,
  argTypes: {
    readonly: {
      defaultValue: false,
      name: "Readonly",
      control: { type: "boolean" },
    },
  },
} as ComponentMeta<typeof AsideStory>;

const Template: ComponentStory<typeof AsideStory> = (args) => (
  <AsideStory readonly={args.readonly} />
);

export const Primary = Template.bind({
  readonly: false,
});
// More on args: https://storybook.js.org/docs/react/writing-stories/args
Primary.args = {};
