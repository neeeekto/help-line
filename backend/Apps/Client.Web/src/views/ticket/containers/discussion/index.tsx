import React from "react";
import { useTicketContext } from "../../context.ticket";
import css from "./discussion.module.scss";
import cn from "classnames";
import { TicketCreatedEventView } from "./ticket-created.event-view";
import { PropsWithClassName } from "@shared/react.types";

export const TicketDiscussion: React.FC<PropsWithClassName> = ({
  className,
}) => {
  const ctx = useTicketContext();

  return (
    <div className={cn(css.box, className)}>
      {ctx.ticket.events.map((x) => {
        switch (x.$type) {
          case "TicketCreatedEventView":
            return <TicketCreatedEventView key={x.id} event={x} />;
          default:
            return <div>{x.$type}</div>;
        }
      })}
    </div>
  );
};
