import React, { PropsWithChildren } from 'react';
import css from './aside.module.scss';
import cn from 'classnames';
import { TicketAssignAside } from './ticket-assign-aside';
import { TicketStatusAside } from './ticket-status';
import { AsideCardItem } from './aside-card';
import { TicketActionsAside } from './ticket-actions-aside';

export const TicketAside = ({
  className,
  children,
}: PropsWithChildren<{ className?: string }>) => {
  return <aside className={cn(className, css.root)}>{children}</aside>;
};

TicketAside.Assign = TicketAssignAside;
TicketAside.Status = TicketStatusAside;
TicketAside.Card = AsideCardItem;
TicketAside.Actions = TicketActionsAside;
