import { ComponentStory, ComponentMeta } from '@storybook/react';
import React, { ComponentProps } from 'react';
import { MigrationsProvider } from './migrations-provider';
import {
  makeStoryFactory,
  StorybookWrapper,
} from '../../../../../libs/modules/storybook/src';
import {
  adminMigrationsStubApi,
  AdminMigrationsStubs,
} from '../../../../../libs/entities/admin/stubs/src/migrations';
import { MswHandlers } from '../../../../../libs/modules/http-stubs/src';
import { within, userEvent } from '@storybook/testing-library';
import { expect } from '@storybook/jest';

export default {
  component: MigrationsProvider,
  title: 'views/MigrationsProvider',
} as ComponentMeta<typeof MigrationsProvider>;

const factory = makeStoryFactory(
  (args: ComponentProps<typeof MigrationsProvider>) => (
    <StorybookWrapper>
      <MigrationsProvider {...args}>Content</MigrationsProvider>
    </StorybookWrapper>
  )
);

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
  play: async ({ canvasElement }) => {
    const canvas = within(canvasElement);
  },
});

export const Loading = factory.create({
  parameters: {
    msw: {
      handlers: [
        adminMigrationsStubApi.get().handle(MswHandlers.delay('infinite')),
      ],
    },
  },
});

export const Error = factory.create({
  parameters: {
    msw: {
      handlers: [adminMigrationsStubApi.get().handle(MswHandlers.error(500))],
    },
  },
});
