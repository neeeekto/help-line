import React, { PropsWithChildren, useEffect, useRef } from 'react';
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

Ticket.Aside = TicketAside;
Ticket.Header = TicketHeader;
Ticket.Discussion = TicketDiscussion;
