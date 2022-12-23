import { createStubFactory } from '@help-line/dev/http-stubs';
import {
  Migration,
  MigrationStatus,
  MigrationStatusBase,
  MigrationStatusType,
} from '@help-line/entities/admin/api';
import { faker } from '@faker-js/faker';

export namespace AdminMigrationsStubs {
  export const createMigrationStatus = createStubFactory<MigrationStatus>(
    () => ({
      dateTime: faker.datatype.datetime().toISOString(),
      $type: MigrationStatusType.InQueue,
    })
  );

  export const createMigration = createStubFactory<Migration>(() => ({
    name: faker.random.word(),
    description: faker.random.words(10),
    children: [],
    parents: [],
    isManual: false,
    applied: false,
    statuses: [createMigrationStatus()],
  }));
}
