import React, { useEffect, useRef } from "react";
import { TicketRootContext, TicketRootContextValue } from "./context.ticket";
import { AsideTicket } from "./containers/aside";
import { HeaderTicket } from "./containers/header";
import { TicketDiscussion } from "./containers/discussion";
import { PropsWithClassName } from "@shared/react.types";

interface TicketProps
  extends PropsWithClassName,
    Omit<TicketRootContextValue, "onExecute"> {
  onExecute?: TicketRootContextValue["onExecute"];
}

export interface TicketInterface extends React.FC<TicketProps> {
  Aside: typeof AsideTicket;
  Header: typeof HeaderTicket;
  Discussion: typeof TicketDiscussion;
}

export const Ticket: TicketInterface = ({
  ticket,
  className,
  children,
  readonly,
  onExecute,
}) => {
  const rootContextValue = useRef<TicketRootContextValue>({
    ticket,
    readonly,
    onExecute: onExecute || (() => Promise.resolve()),
  });
  useEffect(() => {
    rootContextValue.current = {
      ...rootContextValue.current,
      ticket,
      readonly,
      onExecute: onExecute || (() => Promise.resolve()),
    };
  }, [ticket, readonly, onExecute]);
  return (
    <div className={className}>
      <TicketRootContext.Provider value={rootContextValue.current}>
        {children}
      </TicketRootContext.Provider>
    </div>
  );
};

Ticket.Aside = AsideTicket;
Ticket.Header = HeaderTicket;
Ticket.Discussion = TicketDiscussion;
