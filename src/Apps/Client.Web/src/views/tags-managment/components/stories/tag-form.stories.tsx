import React from "react";
import { ComponentStory, ComponentMeta } from "@storybook/react";
import { TagForm } from "../tag-form";
import { Drawer } from "antd";
import { Tag } from "@entities/helpdesk/tags";

export default {
  title: "Tag Management/Tags/Form",
  component: TagForm,
  argTypes: {
    loading: {
      defaultValue: false,
      name: "Loading",
      control: { type: "boolean" },
    },
    tags: {
      defaultValue: [
        {
          key: 'test',
          enabled: true
        } as Tag
      ],
      name: "Tags",
    },
  },
} as ComponentMeta<typeof TagForm>;

const Template: ComponentStory<typeof TagForm> = (args) => (
  <div>
    <Drawer visible placement="left" getContainer={false} width="400px">
      <TagForm {...args} />
    </Drawer>
  </div>
);

export const Primary = Template.bind({
  tags: [],
});
// More on args: https://storybook.js.org/docs/react/writing-stories/args
Primary.args = {};
