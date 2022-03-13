import React, { useEffect } from "react";
import { Ticket } from "@views/ticket";
import cn from "classnames";
import { boxCss, spacingCss } from "@shared/styles";
import {
  useTicketActionMutation,
  useTicketQuery,
} from "@entities/helpdesk/tickets/queries";
import { Spin } from "antd";
import { useSystemStore$ } from "@core/system";
import { observer } from "mobx-react-lite";
import { useTicketEvents } from "@entities/helpdesk/tickets";

export const TicketView: React.FC = observer(() => {
  const systemStore = useSystemStore$();
  const events = useTicketEvents();
  useEffect(() => {
    events.commands.Subscribe("1-0000001");
    const unsub = events.add({
      OnUpdated: ({ newEventsIds }) => {
        console.log(newEventsIds);
      },
    });
    return () => {
      events.commands.Unsubscribe("1-0000001").then(unsub);
    };
  }, [events]);

  const ticketQuery = useTicketQuery(
    "1-0000001",
    systemStore.state.currentProject!
  );
  const ticketActionMutation = useTicketActionMutation(
    "1-0000001",
    systemStore.state.currentProject!
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
});
