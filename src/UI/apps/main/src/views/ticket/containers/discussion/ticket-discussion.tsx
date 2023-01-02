import React from 'react';
import { useTicketContext } from '../../context.ticket';
import css from './discussion.module.scss';
import cn from 'classnames';
import { TicketCreatedEventView } from './events/ticket-creating';
import { TicketEventType } from '@help-line/entities/client/api';

export const TicketDiscussion = ({ className }: { className?: string }) => {
  const ctx = useTicketContext();

  return (
    <div className={cn(css.box, className)}>
      {ctx.ticket.events.map((x) => {
        switch (x.$type) {
          case TicketEventType.Creating:
            return <TicketCreatedEventView key={x.id} event={x} />;
          default:
            return <div>{x.$type}</div>;
        }
      })}
    </div>
  );
};
