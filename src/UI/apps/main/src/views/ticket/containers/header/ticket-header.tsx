import React from 'react';
import css from './header.module.scss';
import cn from 'classnames';
import { TicketNumber } from '../../components/ticket-number';
import { HeaderItem } from './header-item';
import { useTicketContext } from '../../context.ticket';
import { boxCss } from '@help-line/style-utils';
import { TimeAgo } from '@help-line/components';

export const TicketHeader = ({ className }: { className?: string }) => {
  const ctx = useTicketContext();

  return (
    <div
      className={cn(
        boxCss.flex,
        boxCss.alignItemsCenter,
        css.header,
        className
      )}
    >
      <TicketNumber number={ctx.ticket.id} />
      <HeaderItem title="Priority">{ctx.ticket.priority}</HeaderItem>
      <HeaderItem title="Lang">{ctx.ticket.language.toUpperCase()}</HeaderItem>
      <HeaderItem title="Created">
        <TimeAgo value={ctx.ticket!.createDate} />
      </HeaderItem>
      {ctx.ticket.discussionState.lastReplyDate && (
        <HeaderItem title="Last reply">
          <TimeAgo value={ctx.ticket.discussionState.lastReplyDate} />
        </HeaderItem>
      )}
    </div>
  );
};
