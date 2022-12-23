import React from "react";
import css from "./aside.module.scss";
import cn from "classnames";
import { PropsWithClassName } from "@shared/react.types";
import { TicketAssignAside } from "./assign.aside";
import { TicketStatusAside } from "./ticket-status.aside";
import { CardAssign } from "./card.assign";
import { ActionsAside } from "./actions.aside";

export interface AsideTicketInterface extends React.FC<PropsWithClassName> {
  Assign: typeof TicketAssignAside;
  Status: typeof TicketStatusAside;
  Card: typeof CardAssign;
  Actions: typeof ActionsAside;
}

export const AsideTicket: AsideTicketInterface = ({ className, children }) => {
  return <aside className={cn(className, css.aside)}>{children}</aside>;
};

AsideTicket.Assign = TicketAssignAside;
AsideTicket.Status = TicketStatusAside;
AsideTicket.Card = CardAssign;
AsideTicket.Actions = ActionsAside;
