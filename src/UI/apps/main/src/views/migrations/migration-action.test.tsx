import { render, act } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { MigrationAction } from './migration-action';
import { AdminMigrationsStubs } from '@help-line/entities/admin/stubs';
import { UnitTestWrapper } from '@help-line/dev/unit-tests';
import { MigrationStatusType } from '@help-line/entities/admin/api';
import { ComponentProps } from 'react';
import * as mutationQueries from '@help-line/entities/admin/query';
import { expect } from '@storybook/jest';

describe('MigrationAction', () => {
  const renderComponent = (args: ComponentProps<typeof MigrationAction>) =>
    render(
      <UnitTestWrapper>
        <MigrationAction {...args} />
      </UnitTestWrapper>
    );
  describe('rendering', () => {
    it('should show retry btn for auto migration with successful rollback', () => {
      const { getByText } = renderComponent({
        migration: AdminMigrationsStubs.createMigration({
          isManual: false,
          statuses: [
            AdminMigrationsStubs.createMigrationStatus({
              $type: MigrationStatusType.RollbackSuccess,
            }),
          ],
        }),
        migrations: [],
      });
      expect(getByText(/Retry/)).toBeInTheDocument();
    });
    it('should not show retry btn for auto migration without success rollback', () => {
      const { queryByText } = renderComponent({
        migration: AdminMigrationsStubs.createMigration({
          isManual: false,
          statuses: [
            AdminMigrationsStubs.createMigrationStatus({
              $type: MigrationStatusType.InQueue,
            }),
          ],
        }),
        migrations: [],
      });
      expect(queryByText(/Retry/)).not.toBeInTheDocument();
    });
    it('should show run btn for manual migration in queue', () => {
      const { queryByText } = renderComponent({
        migration: AdminMigrationsStubs.createMigration({
          isManual: true,
          statuses: [
            AdminMigrationsStubs.createMigrationStatus({
              $type: MigrationStatusType.InQueue,
            }),
          ],
        }),
        migrations: [],
      });
      expect(queryByText(/Run/)).toBeInTheDocument();
    });
    it('should show run btn for manual migration with success rollback status ', () => {
      const { queryByText } = renderComponent({
        migration: AdminMigrationsStubs.createMigration({
          isManual: true,
          statuses: [
            AdminMigrationsStubs.createMigrationStatus({
              $type: MigrationStatusType.RollbackSuccess,
            }),
          ],
        }),
        migrations: [],
      });
      expect(queryByText(/Run/)).toBeInTheDocument();
    });
    it('should not show run btn for manual migration if parent not applied', () => {
      const parent = AdminMigrationsStubs.createMigration({
        applied: false,
      });
      const { queryByText } = renderComponent({
        migration: AdminMigrationsStubs.createMigration({
          isManual: true,
          statuses: [
            AdminMigrationsStubs.createMigrationStatus({
              $type: MigrationStatusType.RollbackSuccess,
            }),
          ],
          parents: [parent.name],
        }),
        migrations: [parent],
      });
      expect(queryByText(/Run/)).not.toBeInTheDocument();
    });
    it('should disable retry btn if disable prop is true', () => {
      const { getByText } = renderComponent({
        disabled: true,
        migration: AdminMigrationsStubs.createMigration({
          isManual: false,
          statuses: [
            AdminMigrationsStubs.createMigrationStatus({
              $type: MigrationStatusType.RollbackSuccess,
            }),
          ],
        }),
        migrations: [],
      });
      expect(getByText(/Retry/).closest('button')).toBeDisabled();
    });
    it('should disable run btn if disable prop is true', () => {
      const { getByText } = renderComponent({
        disabled: true,
        migration: AdminMigrationsStubs.createMigration({
          isManual: true,
          statuses: [
            AdminMigrationsStubs.createMigrationStatus({
              $type: MigrationStatusType.RollbackSuccess,
            }),
          ],
        }),
        migrations: [],
      });
      expect(getByText(/Run/).closest('button')).toBeDisabled();
    });
  });
  describe('interactions', () => {
    const mutationSpy = jest.spyOn(mutationQueries, 'useRunMigrationMutation');
    const mutateFn = jest.fn();
    beforeEach(() => {
      mutationSpy.mockImplementation(
        () =>
          ({
            mutate: mutateFn,
          } as any)
      );
    });
    afterEach(() => {
      mutationSpy.mockClear();
      mutateFn.mockClear();
    });

    it('should run migration by click on retry btn', () => {
      const migration = AdminMigrationsStubs.createMigration({
        isManual: false,
        statuses: [
          AdminMigrationsStubs.createMigrationStatus({
            $type: MigrationStatusType.RollbackSuccess,
          }),
        ],
      });
      const { getByText } = renderComponent({
        migration,
        migrations: [],
      });
      act(() => {
        userEvent.click(getByText(/Retry/));
      });

      expect(mutationSpy).toBeCalledWith(migration.name);
      expect(mutateFn).toBeCalledWith(undefined);
    });
    it('should run migration by click on run btn', () => {
      const migration = AdminMigrationsStubs.createMigration({
        isManual: true,
        statuses: [
          AdminMigrationsStubs.createMigrationStatus({
            $type: MigrationStatusType.InQueue,
          }),
        ],
      });
      const { getByText } = renderComponent({
        migration,
        migrations: [],
      });
      act(() => {
        userEvent.click(getByText(/Run/));
      });

      expect(mutationSpy).toBeCalledWith(migration.name);
      expect(mutateFn).toBeCalledWith(undefined);
    });
    it('should show params modal by click on run btn for migration with params', () => {
      const migration = AdminMigrationsStubs.createMigration({
        isManual: true,
        params: 'test',
        statuses: [
          AdminMigrationsStubs.createMigrationStatus({
            $type: MigrationStatusType.InQueue,
          }),
        ],
      });
      const { getByTestId, getByText } = renderComponent({
        migration,
        migrations: [],
      });
      act(() => {
        userEvent.click(getByText(/Run/));
      });
      expect(getByTestId('modalParamsForm')).toBeInTheDocument();
    });

    it('should run migration after ending of params editing', () => {
      const migration = AdminMigrationsStubs.createMigration({
        isManual: true,
        params: 'test',
        statuses: [
          AdminMigrationsStubs.createMigrationStatus({
            $type: MigrationStatusType.InQueue,
          }),
        ],
      });
      const { getByText } = renderComponent({
        migration,
        migrations: [],
      });
      act(() => {
        userEvent.click(getByText(/Run/));
      });
      act(() => {
        userEvent.click(getByText(/OK/));
      });
      expect(mutationSpy).toBeCalledWith(migration.name);
      expect(mutateFn).toBeCalledWith(
        expect.objectContaining({
          $type: migration.params,
        })
      );
    });
  });
  describe('lifecycle', () => {});
});
