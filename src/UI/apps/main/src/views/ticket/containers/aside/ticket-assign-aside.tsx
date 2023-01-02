import css from './aside.module.scss';
import cn from 'classnames';
import React, { PropsWithChildren, useCallback } from 'react';
import { useTicketContext } from '../../context.ticket';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { Avatar, Button, Select, Spin, Switch, Typography } from 'antd';
import { AsideCardItem } from './aside-card';
import { useBoolean } from 'ahooks';
import { observer } from 'mobx-react-lite';

import { LoadingOutlined } from '@ant-design/icons';
import { useAuthProfile } from '@help-line/modules/auth';
import { useUserQuery, useUsersQuery } from '@help-line/entities/client/query';
import { Operator, Project, UserStatus } from '@help-line/entities/client/api';

const HardAssign: React.FC = () => {
  const ctx = useTicketContext();
  const [loading, loadingActions] = useBoolean();

  const onToggle = useCallback(async () => {
    try {
      loadingActions.setTrue();
      await ctx.onExecute({
        $type: 'ToggleHardAssigmentAction',
        hardAssigment: !ctx.ticket.hardAssigment,
      });
    } finally {
      loadingActions.setFalse();
    }
  }, [ctx, loadingActions]);

  return (
    <div
      className={cn(boxCss.flex, boxCss.alignItemsCenter, spacingCss.spaceSm)}
    >
      <Switch
        checked={ctx.ticket.hardAssigment}
        size="small"
        onClick={onToggle}
        loading={loading}
      />
      Permanently assign
    </div>
  );
};

const AssignActions: React.FC<{
  loading?: boolean;
  operatorId?: string;
  openAssignForm?: () => void;
  onAssignToMe?: () => void;
  className?: string;
}> = observer(
  ({ openAssignForm, onAssignToMe, loading, operatorId, className }) => {
    const authProfile = useAuthProfile();
    return (
      <div className={cn(boxCss.flex, spacingCss.spaceSm, className)}>
        {operatorId !== authProfile?.userId && onAssignToMe && (
          <Button
            size="small"
            type="primary"
            ghost
            onClick={onAssignToMe}
            disabled={loading}
          >
            Assign to me
          </Button>
        )}
        <Button
          size="small"
          type="primary"
          ghost
          onClick={openAssignForm}
          disabled={loading}
        >
          {operatorId ? 'Reassign' : 'Assign to'}
        </Button>
      </div>
    );
  }
);

const CurrentOperator = ({
  operatorId,
  children,
}: PropsWithChildren<{
  operatorId: string;
}>) => {
  const user = useUserQuery(operatorId);
  return (
    <div
      className={cn(
        boxCss.flex,
        spacingCss.spaceSm,
        boxCss.alignItemsCenter,
        spacingCss.marginTopSm
      )}
    >
      {user.isLoading ? (
        <Spin />
      ) : (
        <Avatar size="default" src={user.data?.info.photo} />
      )}
      <Typography.Text>
        {user.data?.info.firstName} {user.data?.info.lastName}
      </Typography.Text>
      {children}
    </div>
  );
};

const CurrentSelected: React.FC<{ operatorId: string }> = ({ operatorId }) => {
  const user = useUserQuery(operatorId);
  return (
    <>
      {user.data?.info.firstName} {user.data?.info.lastName}
    </>
  );
};

const AssignForm = ({
  operatorId,
  className,
  onSelect,
  loading,
  projectId,
}: {
  projectId: Project['id'];
  operatorId?: string;
  loading?: boolean;
  onSelect?: (operatorId: Operator['id']) => void;
  className?: string;
}) => {
  const usersQuery = useUsersQuery(projectId);
  return (
    <Select
      size="small"
      onSelect={onSelect}
      value={operatorId}
      loading={usersQuery.isLoading}
      disabled={loading}
    >
      {!!operatorId && (
        <Select.Option key={operatorId} value={operatorId} disabled>
          <CurrentSelected operatorId={operatorId} />
        </Select.Option>
      )}
      {usersQuery.data
        ?.filter((x) => x.status === UserStatus.Active && x.id !== operatorId)
        .map((x) => (
          <Select.Option key={x.id} value={x.id}>
            <Avatar size="default" src={x.info.photo} />
            {x.info.firstName} {x.info.lastName}
          </Select.Option>
        ))}
    </Select>
  );
};

export const TicketAssignAside = () => {
  const [loading, setLoadingActions] = useBoolean();
  const [showForm, showFormActions] = useBoolean();
  const ctx = useTicketContext();
  const authProfile = useAuthProfile();

  const assignTo = useCallback(
    async (operatorId: Operator['id']) => {
      try {
        setLoadingActions.setTrue();
        await ctx.onExecute({ $type: 'AssignAction', operatorId: operatorId });
        showFormActions.setFalse();
      } finally {
        setLoadingActions.setFalse();
      }
    },
    [ctx, setLoadingActions, showFormActions]
  );
  const myId = authProfile?.userId;
  const assignToMe = useCallback(async () => {
    if (myId) {
      await assignTo(myId);
    }
  }, [assignTo, myId]);
  const onAssignToMe = myId ? assignToMe : void 0;

  return (
    <AsideCardItem
      className={cn(boxCss.flex, boxCss.flexColumn, spacingCss.spaceSm)}
    >
      <div
        className={cn(
          boxCss.flex,
          boxCss.justifyContentSpaceBetween,
          boxCss.alignItemsCenter
        )}
      >
        <span className={css.title}>
          Assignee
          {loading && <LoadingOutlined className={spacingCss.marginLeftSm} />}
        </span>
        {ctx.ticket.assignedTo ? (
          <HardAssign />
        ) : !ctx.readonly ? (
          <AssignActions
            onAssignToMe={onAssignToMe}
            loading={loading}
            operatorId={ctx.ticket.assignedTo}
          />
        ) : null}
      </div>
      {ctx.ticket.assignedTo && (
        <CurrentOperator operatorId={ctx.ticket.assignedTo}>
          {!ctx.readonly && (
            <div className={spacingCss.marginLeftAuto}>
              {showForm ? (
                <Button
                  size="small"
                  onClick={showFormActions.setFalse}
                  disabled={loading}
                >
                  Cancel
                </Button>
              ) : (
                <AssignActions
                  onAssignToMe={onAssignToMe}
                  loading={loading}
                  operatorId={ctx.ticket.assignedTo}
                  openAssignForm={showFormActions.setTrue}
                />
              )}
            </div>
          )}
        </CurrentOperator>
      )}
      {!ctx.readonly && showForm && (
        <AssignForm
          projectId={ctx.ticket.projectId}
          operatorId={ctx.ticket.assignedTo}
          onSelect={assignTo}
          loading={loading}
        />
      )}
    </AsideCardItem>
  );
};
