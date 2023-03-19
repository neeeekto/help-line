import { ComponentMeta } from '@storybook/react';
import React, { ComponentProps } from 'react';
import { MigrationsProvider } from './migrations-provider';
import { makeStoryFactory } from '../../../../../libs/dev/storybook/src';
import {
  adminMigrationsStubApi,
  AdminMigrationsStubs,
} from '../../../../../libs/entities/admin/stubs/src';
import { MswHandlers } from '../../../../../libs/dev/http-stubs/src';
import { MigrationStatusType } from '../../../../../libs/entities/admin/api/src';

export default {
  component: MigrationsProvider,
  title: 'views/MigrationsProvider',
} as ComponentMeta<typeof MigrationsProvider>;

const factory = makeStoryFactory(
  (args: ComponentProps<typeof MigrationsProvider>) => (
    <MigrationsProvider {...args}>Content</MigrationsProvider>
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
});

export const AllTypes = factory.create({
  parameters: {
    msw: {
      handlers: [
        adminMigrationsStubApi.get().handle(
          MswHandlers.success([
            AdminMigrationsStubs.createMigration({
              isManual: true,
              statuses: [AdminMigrationsStubs.createMigrationStatus({})],
            }),
            AdminMigrationsStubs.createMigration({
              isManual: false,
              statuses: [
                AdminMigrationsStubs.createMigrationStatus({
                  $type: MigrationStatusType.InQueue,
                  dateTime: new Date(Date.now() - 1000).toISOString(),
                }),
                AdminMigrationsStubs.createMigrationStatus({
                  $type: MigrationStatusType.Error,
                  dateTime: new Date(Date.now()).toISOString(),
                }),
              ],
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
