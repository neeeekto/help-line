import React from "react";
import { useTicketContext } from "@views/ticket/context.ticket";
import css from "./header.module.scss";
import cn from "classnames";
import { TicketNumber } from "../../components/ticket-number";
import { boxCss } from "@shared/styles";
import { TimeAgo } from "@shared/components/time-ago";
import { PropsWithClassName } from "@shared/react.types";
import { HeaderItem } from "./header-item";

export const HeaderTicket: React.FC<PropsWithClassName> = ({ className }) => {
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
