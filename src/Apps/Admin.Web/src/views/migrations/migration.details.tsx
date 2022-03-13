import React from "react";
import { Migration } from "@entities/migrations";
import { Alert, Timeline, Typography } from "antd";
import ReactTimeago from "react-timeago";
import ReactJson from "react-json-view";

export const MigrationDetails: React.FC<{ migration: Migration }> = ({
  migration,
}) => {
  return (
    <Timeline>
      {migration.statuses.map((s) => {
        let color = "gray";
        let details: any;
        switch (s.$type) {
          case "MigrationExecutingStatus":
            break;
          case "MigrationErrorStatus":
          case "MigrationRollbackErrorStatus":
            color = "red";
            details = s.exception;
            break;
          case "MigrationAppliedAndSavedStatus":
          case "MigrationAppliedStatus":
            color = "green";
            break;
          case "MigrationInQueueStatus":
            break;
          case "MigrationRollbackStatus":
          case "MigrationRollbackSuccessStatus":
            color = "blue";
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
                    message={
                      <ReactJson
                        displayDataTypes={false}
                        displayObjectSize={false}
                        src={details || {}}
                      />
                    }
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
