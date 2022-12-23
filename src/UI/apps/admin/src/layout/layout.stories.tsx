import { ComponentMeta } from '@storybook/react';
import { MemoryRouter } from 'react-router-dom';
import {
  makeStoryFactory,
  StorybookWrapper,
} from '../../../../libs/dev/storybook/src';
import React, { ComponentProps } from 'react';
import {
  adminMigrationsStubApi,
  AdminMigrationsStubs,
} from '../../../../libs/entities/admin/stubs/src';
import { MswHandlers } from '../../../../libs/dev/http-stubs/src';
import { LayoutRoot } from './index';

export default {
  component: LayoutRoot,
  title: 'layout/LayoutRoot',
} as ComponentMeta<typeof LayoutRoot>;

const factory = makeStoryFactory((args: ComponentProps<typeof LayoutRoot>) => (
  <StorybookWrapper>
    <LayoutRoot {...args}>Content</LayoutRoot>
  </StorybookWrapper>
));

export const Primary = factory.create({
  parameters: {
    msw: {
      handlers: [
        adminMigrationsStubApi.get().handle(
          MswHandlers.success([
            AdminMigrationsStubs.createMigration({
              statuses: [AdminMigrationsStubs.createMigrationStatus({})],
            }),
          ])
        ),
      ],
    },
  },
});
