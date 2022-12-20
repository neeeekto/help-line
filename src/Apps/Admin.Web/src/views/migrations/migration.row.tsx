import React from "react";
import { Migration } from "@entities/migrations";
import cn from "classnames";
import { boxCss, mouseCss, spacingCss, utilsCss } from "@shared/styles";
import {
  CheckCircleOutlined,
  CloseCircleOutlined,
  CloudServerOutlined,
  FieldTimeOutlined,
  LoadingOutlined,
  RollbackOutlined,
} from "@ant-design/icons";
import last from "lodash/last";
import { Drawer, Dropdown, Tag, Tooltip, Typography } from "antd";
import ReactTimeago from "react-timeago";

export const MigrationRow: React.FC<{
  migration: Migration;
  onClick?: () => void;
}> = ({ migration, children, onClick }) => {
  const lastStatus = last(migration.statuses);
  return (
    <>
      <div
        className={cn(
          boxCss.fullWidth,
          boxCss.fullHeight,
          boxCss.flex,
          boxCss.alignItemsCenter,
          spacingCss.spaceXs
        )}
      >
        <span className={mouseCss.lowAttention}>
          {migration.isManual ? (
            <Tag color="warning">Manual</Tag>
          ) : (
            <Tag color="processing">Auto</Tag>
          )}
        </span>

        <Tooltip
          title={
            <div>
              {lastStatus?.$type} <br />
              <ReactTimeago date={lastStatus!.dateTime} />
            </div>
          }
          mouseEnterDelay={0.5}
        >
          {lastStatus?.$type === "MigrationInQueueStatus" && (
            <Typography.Text type="warning">
              <FieldTimeOutlined />
            </Typography.Text>
          )}
          {lastStatus?.$type === "MigrationExecutingStatus" && (
            <LoadingOutlined />
          )}
          {lastStatus?.$type === "MigrationRollbackStatus" && (
            <Typography.Text type="warning">
              <LoadingOutlined />
              <RollbackOutlined />
            </Typography.Text>
          )}

          {lastStatus?.$type === "MigrationRollbackSuccessStatus" && (
            <Typography.Text type="warning">
              <RollbackOutlined />
            </Typography.Text>
          )}

          {lastStatus?.$type === "MigrationErrorStatus" && (
            <Typography.Text type="danger">
              <CloseCircleOutlined />
            </Typography.Text>
          )}
          {lastStatus?.$type === "MigrationRollbackErrorStatus" && (
            <Typography.Text type="danger">
              <RollbackOutlined />
            </Typography.Text>
          )}
          {lastStatus?.$type === "MigrationAppliedAndSavedStatus" && (
            <Typography.Text type="success">
              <CloudServerOutlined />
            </Typography.Text>
          )}
          {lastStatus?.$type === "MigrationAppliedStatus" && (
            <Typography.Text type="success">
              <CheckCircleOutlined />
            </Typography.Text>
          )}
        </Tooltip>

        <Typography.Text strong onClick={onClick} className={mouseCss.pointer}>
          {migration.name}
        </Typography.Text>
        <div className={spacingCss.marginLeftAuto}>{children}</div>
      </div>
    </>
  );
};
