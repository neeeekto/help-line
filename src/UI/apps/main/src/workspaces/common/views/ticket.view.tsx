import React, { useEffect } from 'react';
import cn from 'classnames';

import { Spin } from 'antd';
import { useParams } from 'react-router-dom';
import { TicketId, useTicketEvents } from '@help-line/entities/client/api';
import {
  useExecuteTicketMutation,
  useTicketQuery,
} from '../../../../../../libs/entities/client/query/src/helpdesk/ticket';
import { Ticket } from '../../../views/ticket';
import { boxCss, spacingCss } from '@help-line/style-utils';

export const TicketView = () => {
  const { ticketId } = useParams<{ ticketId: TicketId }>();
  const events = useTicketEvents();
  useEffect(() => {
    events.commands.Subscribe(ticketId!);
    const unsub = events.add({
      OnUpdated: ({ newEventsIds }) => {
        console.log(newEventsIds);
      },
    });
    return () => {
      events.commands.Unsubscribe(ticketId!).then(unsub);
    };
  }, [events, ticketId]);

  const ticketQuery = useTicketQuery(ticketId!);
  const ticketActionMutation = useExecuteTicketMutation(
    ticketId!,
    ticketQuery.data?.version || 0
  );
  if (ticketQuery.isLoading) {
    return <Spin />;
  }
  if (!ticketQuery.data) {
    return <span>No ticket</span>;
  }
  return (
    <Ticket
      ticket={ticketQuery.data!}
      onExecute={(act) => ticketActionMutation.mutateAsync([act])}
      className={cn(
        boxCss.flex,
        boxCss.flexColumn,
        boxCss.fullHeight,
        boxCss.fullWidth,
        boxCss.overflowHidden,
        spacingCss.spaceSm
      )}
    >
      <Ticket.Header className={boxCss.flex00Auto} />
      <div
        className={cn(
          boxCss.flex,
          spacingCss.spaceSm,
          boxCss.fullWidth,
          boxCss.fullHeight
        )}
      >
        <Ticket.Discussion
          className={cn(boxCss.fullHeight, boxCss.fullWidth)}
        />
        <Ticket.Aside className={boxCss.flex00Auto}>
          <Ticket.Aside.Status />
          <Ticket.Aside.Assign />
          <Ticket.Aside.Actions />
        </Ticket.Aside>
      </div>
    </Ticket>
  );
};
