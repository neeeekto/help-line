import React, { useCallback, useMemo } from 'react';
import { AsideCardItem } from './aside-card';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';
import css from './aside.module.scss';
import { Button, Modal, Switch, Tooltip } from 'antd';
import { useBoolean } from 'ahooks';
import { useTicketContext } from '../../context.ticket';
import {
  TicketActionUtils,
  TicketStatusKind,
} from '@help-line/entities/client/api';

const RejectButton: React.FC = () => {
  const ctx = useTicketContext();
  const [showPopup, showPopupActions] = useBoolean();
  const canReject = useMemo(
    () => TicketActionUtils.checkCanReject(ctx.ticket),
    [ctx.ticket]
  );

  if (!canReject) {
    return null;
  }

  return (
    <>
      <Button
        size="small"
        type="primary"
        danger
        onClick={showPopupActions.setTrue}
      >
        Reject
      </Button>
      <Modal
        title="Basic Modal"
        visible={showPopup}
        onOk={showPopupActions.setFalse}
        onCancel={showPopupActions.setFalse}
      >
        <p>Some contents...</p>
        <p>Some contents...</p>
        <p>Some contents...</p>
      </Modal>
    </>
  );
};

const PendingToggle: React.FC = () => {
  const ctx = useTicketContext();
  const [loading, loadingActions] = useBoolean(false);
  const isPending = ctx.ticket.status.kind === TicketStatusKind.Pending;
  const hasResolvingReminder = useMemo(
    () =>
      ctx.ticket.events.some(
        (x) =>
          x.$type === 'TicketReminderEventView' &&
          !x.result &&
          x.reminder.resolving
      ),
    [ctx.ticket]
  );
  const togglePending = useCallback(async () => {
    try {
      loadingActions.setTrue();
      await ctx.onExecute({
        $type: 'TogglePendingAction',
        pending: ctx.ticket.status.kind !== TicketStatusKind.Pending,
      });
    } finally {
      loadingActions.setFalse();
    }
  }, [ctx, loadingActions]);

  return (
    <Tooltip
      title="Temporary withdrawal of a ticket from the life cycle"
      mouseEnterDelay={3}
      className={cn(
        boxCss.inlineFlex,
        boxCss.alignItemsCenter,
        spacingCss.spaceSm
      )}
    >
      <Switch
        checked={isPending}
        size="small"
        onClick={togglePending}
        loading={loading}
        disabled={hasResolvingReminder}
      />
      Pending mode
    </Tooltip>
  );
};

export const TicketActionsAside: React.FC = () => {
  const ctx = useTicketContext();

  if (ctx.readonly) {
    return null;
  }
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
        <span className={css.title}>Actions</span>
        {ctx.ticket.status.kind !== TicketStatusKind.Closed && (
          <PendingToggle />
        )}
      </div>
      <div className={cn(boxCss.flex, spacingCss.spaceSm)}>
        <RejectButton />
        <Button size="small" type="primary">
          Resolve
        </Button>
        <Button size="small" danger>
          Reopen
        </Button>
        <Button size="small">Improve</Button>
      </div>
    </AsideCardItem>
  );
};
