import React from 'react';
import { Migration, MigrationStatusType } from '@help-line/entities/admin/api';
import { Alert, Timeline, Typography } from 'antd';
import ReactTimeago from 'react-timeago';

export const MigrationDetails: React.FC<{ migration: Migration }> = ({
  migration,
}) => {
  return (
    <Timeline>
      {migration.statuses.map((s) => {
        let color = 'gray';
        let details: any;
        switch (s.$type) {
          case MigrationStatusType.Executing:
            break;
          case MigrationStatusType.Error:
          case MigrationStatusType.RollbackError:
            color = 'red';
            details = s.exception;
            break;
          case MigrationStatusType.AppliedAndSaved:
          case MigrationStatusType.Applied:
            color = 'green';
            break;
          case MigrationStatusType.InQueue:
            break;
          case MigrationStatusType.Rollback:
          case MigrationStatusType.RollbackSuccess:
            color = 'blue';
            break;
        }
        return (
          <Timeline.Item key={s.dateTime} color={color}>
            <p>{s.$type}</p>
            <p>
              <ReactTimeago date={s.dateTime!} />
            </p>
            {details && (
              <div>
                <div>
                  <Typography.Text type="danger">Error:</Typography.Text>
                </div>
                <div>
                  <Alert
                    message={JSON.stringify(details, null, 2)}
                    type="error"
                  />
                </div>
              </div>
            )}
          </Timeline.Item>
        );
      })}
    </Timeline>
  );
};
