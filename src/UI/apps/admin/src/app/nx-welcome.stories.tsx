import { ComponentStory, ComponentMeta } from '@storybook/react';
import { NxWelcome } from './nx-welcome';

export default {
  component: NxWelcome,
  title: 'NxWelcome',
} as ComponentMeta<typeof NxWelcome>;

const Template: ComponentStory<typeof NxWelcome> = (args) => (
  <NxWelcome {...args} />
);

export const Primary = Template.bind({});
Primary.args = {};
