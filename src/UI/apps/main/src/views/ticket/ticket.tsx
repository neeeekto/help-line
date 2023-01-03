import React, { PropsWithChildren, useEffect, useMemo, useRef } from 'react';
import { TicketRootContext, TicketRootContextValue } from './context.ticket';
import { TicketAside } from './containers/aside';
import { TicketHeader } from './containers/header';
import { TicketDiscussion } from './containers/discussion';

type TicketProps = PropsWithChildren<
  Omit<TicketRootContextValue, 'onExecute'> & {
    onExecute?: TicketRootContextValue['onExecute'];
    className?: string;
  }
>;

export const Ticket = ({
  ticket,
  className,
  children,
  readonly,
  onExecute,
}: TicketProps) => {
  const ctxVal = useMemo(
    () => ({
      ticket,
      readonly,
      onExecute: onExecute || (() => Promise.resolve()),
    }),
    [ticket, readonly, onExecute]
  );
  return (
    <div className={className}>
      <TicketRootContext.Provider value={ctxVal}>
        {children}
      </TicketRootContext.Provider>
    </div>
  );
};

Ticket.Aside = TicketAside;
Ticket.Header = TicketHeader;
Ticket.Discussion = TicketDiscussion;
