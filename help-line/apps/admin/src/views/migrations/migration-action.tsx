import React, { useState } from 'react';
import { Migration } from '@help-line/entities/admin/api';
import last from 'lodash/last';
import { Button, Modal } from 'antd';
import { useRunMigrationMutation } from '@help-line/entities/admin/query';
import { MigrationParamsBuilder } from './migration-prarams';
import { WithType } from '@help-line/entities/share';

export const MigrationAction: React.FC<{
  migration: Migration;
  migrations: Migration[];
}> = ({ migration, migrations }) => {
  const runMigration = useRunMigrationMutation(migration.name);
  const lastStatus = last(migration.statuses);
  const canRetry = lastStatus!.$type === 'MigrationRollbackSuccessStatus';
  const hasRunning = migrations.some((m) =>
    ['MigrationExecutingStatus', 'MigrationRollbackStatus'].includes(
      last(m.statuses)?.$type || ''
    )
  );
  const canRunManual =
    migration.isManual &&
    migration.parents?.every(
      (x) => migrations.find((m) => m.name === x)?.applied
    );
  const [showModal, setShowModal] = useState(false);
  const [params, setParams] = useState<WithType<string>>({
    $type: migration.params || '',
  });
  return (
    <>
      {canRetry && !migration.isManual && (
        <Button
          disabled={hasRunning}
          size="small"
          onClick={() => runMigration.mutate(void 0)}
        >
          Retry
        </Button>
      )}
      {(canRetry || lastStatus!.$type === 'MigrationInQueueStatus') &&
        canRunManual && (
          <Button
            disabled={hasRunning}
            size="small"
            onClick={() =>
              migration.params
                ? setShowModal(true)
                : runMigration.mutate(void 0)
            }
          >
            Run
          </Button>
        )}
      <Modal
        visible={showModal}
        onCancel={() => setShowModal(false)}
        closable={false}
        title={`Set params for ${migration.name}`}
        onOk={() => {
          runMigration.mutate(params);
          setShowModal(false);
        }}
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
