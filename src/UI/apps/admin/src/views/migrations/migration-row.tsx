import React, { PropsWithChildren } from 'react';
import { Migration, MigrationStatusType } from '@help-line/entities/admin/api';
import cn from 'classnames';
import { boxCss, mouseCss, spacingCss } from '@help-line/style-utils';
import {
  CheckCircleOutlined,
  CloseCircleOutlined,
  CloudServerOutlined,
  FieldTimeOutlined,
  LoadingOutlined,
  RollbackOutlined,
} from '@ant-design/icons';
import last from 'lodash/last';
import { Tag, Tooltip, Typography } from 'antd';
import ReactTimeago from 'react-timeago';

export const MigrationRow: React.FC<
  PropsWithChildren<{
    migration: Migration;
    onClick?: () => void;
  }>
> = ({ migration, children, onClick }) => {
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
          {lastStatus?.$type === MigrationStatusType.InQueue && (
            <Typography.Text type="warning">
              <FieldTimeOutlined />
            </Typography.Text>
          )}
          {lastStatus?.$type === MigrationStatusType.Executing && (
            <LoadingOutlined />
          )}
          {lastStatus?.$type === MigrationStatusType.Rollback && (
            <Typography.Text type="warning">
              <LoadingOutlined />
              <RollbackOutlined />
            </Typography.Text>
          )}

          {lastStatus?.$type === MigrationStatusType.RollbackSuccess && (
            <Typography.Text type="warning">
              <RollbackOutlined />
            </Typography.Text>
          )}

          {lastStatus?.$type === MigrationStatusType.Error && (
            <Typography.Text type="danger">
              <CloseCircleOutlined />
            </Typography.Text>
          )}
          {lastStatus?.$type === MigrationStatusType.RollbackError && (
            <Typography.Text type="danger">
              <RollbackOutlined />
            </Typography.Text>
          )}
          {lastStatus?.$type === MigrationStatusType.AppliedAndSaved && (
            <Typography.Text type="success">
              <CloudServerOutlined />
            </Typography.Text>
          )}
          {lastStatus?.$type === MigrationStatusType.Applied && (
            <Typography.Text type="success">
              <CheckCircleOutlined />
            </Typography.Text>
          )}
        </Tooltip>

        <Typography.Text
          strong
          onClick={onClick}
          className={cn(mouseCss.pointer, boxCss.fullWidth)}
        >
          {migration.name}
        </Typography.Text>
        <div className={spacingCss.marginLeftAuto}>{children}</div>
      </div>
    </>
  );
};
