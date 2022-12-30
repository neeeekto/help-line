import { ComponentMeta } from '@storybook/react';
import {
  makeStoryFactory,
  StorybookWrapper,
} from '../../../../../libs/dev/storybook/src';
import React, { ComponentProps } from 'react';
import { LayoutRoot } from '../../layout';
import { Projects } from './projects';
import {
  adminProjectsStubApi,
  AdminProjectStubs,
} from '../../../../../libs/entities/admin/stubs/src';
import { MswHandlers } from '../../../../../libs/dev/http-stubs/src';

export default {
  component: Projects,
  title: 'views/Projects',
} as ComponentMeta<typeof Projects>;

const factory = makeStoryFactory((args: ComponentProps<typeof Projects>) => (
  <StorybookWrapper>
    <LayoutRoot>
      <Projects {...args} />
    </LayoutRoot>
  </StorybookWrapper>
));

export const Primary = factory.create({
  parameters: {
    msw: {
      handlers: [
        adminProjectsStubApi.get().handle(
          MswHandlers.success([
            AdminProjectStubs.createProject({}),
            AdminProjectStubs.createProject({
              active: false,
            }),
          ])
        ),
        adminProjectsStubApi
          .activate({ projectId: '*' })
          .handle(MswHandlers.delay(2000)),
        adminProjectsStubApi
          .deactivate({ projectId: '*' })
          .handle(MswHandlers.delay(2000)),
        adminProjectsStubApi.create({}).handle(MswHandlers.delay(2000)),
        adminProjectsStubApi
          .update({ projectId: '*' })
          .handle(MswHandlers.delay(2000)),
      ],
    },
  },
});
