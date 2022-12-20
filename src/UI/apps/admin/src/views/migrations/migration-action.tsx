import React, { useCallback, useMemo, useState } from 'react';
import { Migration, MigrationStatusType } from '@help-line/entities/admin/api';
import last from 'lodash/last';
import { Button, Modal } from 'antd';
import { useRunMigrationMutation } from '@help-line/entities/admin/query';
import { MigrationParamsBuilder } from './migration-prarams';
import { WithType } from '@help-line/entities/share';
import { useBoolean } from 'ahooks';

interface MigrationActionProps {
  migration: Migration;
  migrations: Migration[];
  disabled?: boolean;
}

export const MigrationAction: React.FC<MigrationActionProps> = ({
  migration,
  migrations,
  disabled,
}) => {
  const runMigration = useRunMigrationMutation(migration.name);
  const [showModal, showModalActions] = useBoolean(false);
  const [params, setParams] = useState<WithType<string>>({
    $type: migration.params || '',
  });

  const [showRetryAction, showRunAction] = useMemo(() => {
    const lastStatus = last(migration.statuses);
    const canRetry = lastStatus!.$type === MigrationStatusType.RollbackSuccess;
    const showRetryAction = canRetry && !migration.isManual;
    const canRunManual =
      migration.isManual &&
      migration.parents?.every(
        (x) => migrations.find((m) => m.name === x)?.applied
      );
    const showRunAction =
      (canRetry || lastStatus!.$type === MigrationStatusType.InQueue) &&
      canRunManual;

    return [showRetryAction, showRunAction];
  }, [migration, migrations]);

  const onRetry = useCallback(() => {
    runMigration.mutate(undefined);
  }, [runMigration]);

  const onRunOrOpenParamFormModal = useCallback(() => {
    migration.params ? showModalActions.setTrue() : runMigration.mutate(void 0);
  }, [migration, runMigration]);

  const onRunWithParams = useCallback(() => {
    runMigration.mutate(params);
    showModalActions.setFalse();
  }, [runMigration]);

  return (
    <>
      {showRetryAction && (
        <Button disabled={disabled} size="small" onClick={onRetry}>
          Retry
        </Button>
      )}
      {showRunAction && (
        <Button
          disabled={disabled}
          size="small"
          onClick={onRunOrOpenParamFormModal}
        >
          Run
        </Button>
      )}
      <Modal
        data-testid="modalParamsForm"
        open={showModal}
        onCancel={showModalActions.setFalse}
        closable={false}
        title={`Set params for ${migration.name}`}
        onOk={onRunWithParams}
      >
        <MigrationParamsBuilder
          migration={migration}
          params={params}
          onChange={setParams}
        />
      </Modal>
    </>
  );
};
