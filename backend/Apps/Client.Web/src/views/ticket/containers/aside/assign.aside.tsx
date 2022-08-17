import css from "./aside.module.scss";
import cn from "classnames";
import React, { useCallback } from "react";
import { useTicketContext } from "../../context.ticket";
import { boxCss, spacingCss } from "@shared/styles";
import { Avatar, Button, Select, Spin, Switch, Typography } from "antd";
import { CardAssign } from "./card.assign";
import { useBoolean } from "ahooks";
import { observer } from "mobx-react-lite";

import { useSystemStore$ } from "@core/system";
import { PropsWithClassName } from "@shared/react.types";
import {
  OperatorView,
  useOperatorViewQuery,
  useOperatorsViewQuery,
} from "@entities/helpdesk/operators";
import { LoadingOutlined } from "@ant-design/icons";
import { useAuthStore$ } from "@core/auth";

const HardAssign: React.FC = () => {
  const ctx = useTicketContext();
  const [loading, loadingActions] = useBoolean();

  const onToggle = useCallback(async () => {
    try {
      loadingActions.setTrue();
      await ctx.onExecute({
        $type: "ToggleHardAssigmentAction",
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
    const authStore = useAuthStore$();
    return (
      <div className={cn(boxCss.flex, spacingCss.spaceSm, className)}>
        {operatorId !== authStore.state?.me?.id && onAssignToMe && (
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
          {operatorId ? "Reassign" : "Assign to"}
        </Button>
      </div>
    );
  }
);

const CurrentOperator: React.FC<{
  operatorId: string;
}> = observer(({ operatorId, children }) => {
  const user = useOperatorViewQuery(operatorId);
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
        <Avatar size="default" src={user.data?.photo} />
      )}
      <Typography.Text>
        {user.data?.firstName} {user.data?.lastName}
      </Typography.Text>
      {children}
    </div>
  );
});

const CurrentSelected: React.FC<{ operatorId: string }> = ({ operatorId }) => {
  const user = useOperatorViewQuery(operatorId);
  return (
    <>
      {user.data?.firstName} {user.data?.lastName}
    </>
  );
};

const AssignForm: React.FC<
  PropsWithClassName & {
    operatorId?: string;
    loading?: boolean;
    onSelect?: (operatorId: OperatorView["id"]) => void;
  }
> = observer(({ operatorId, className, onSelect, loading }) => {
  const systemStore = useSystemStore$();
  const usersQuery = useOperatorsViewQuery(systemStore.state?.currentProject!);
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
        ?.filter((x) => x.active && x.id !== operatorId)
        .map((x) => (
          <Select.Option key={x.id} value={x.id}>
            <Avatar size="default" src={x.photo} />
            {x.firstName} {x.lastName}
          </Select.Option>
        ))}
    </Select>
  );
});

export const TicketAssignAside: React.FC = observer(() => {
  const [loading, setLoadingActions] = useBoolean();
  const [showForm, showFormActions] = useBoolean();
  const ctx = useTicketContext();
  const authStore = useAuthStore$();

  const assignTo = useCallback(
    async (operatorId: OperatorView["id"]) => {
      try {
        setLoadingActions.setTrue();
        await ctx.onExecute({ $type: "AssignAction", operatorId: operatorId });
        showFormActions.setFalse();
      } finally {
        setLoadingActions.setFalse();
      }
    },
    [ctx, setLoadingActions, showFormActions]
  );
  const myId = authStore.state?.me?.id;
  const assignToMe = useCallback(async () => {
    if (myId) {
      await assignTo(myId);
    }
  }, [assignTo, myId]);
  const onAssignToMe = myId ? assignToMe : void 0;

  return (
    <CardAssign
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
          operatorId={ctx.ticket.assignedTo}
          onSelect={assignTo}
          loading={loading}
        />
      )}
    </CardAssign>
  );
});
