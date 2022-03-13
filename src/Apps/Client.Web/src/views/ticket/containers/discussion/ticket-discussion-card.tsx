import React from "react";
import css from "./discussion.module.scss";
import cn from "classnames";

export const TicketDiscussionCard: React.FC<{
  className?: string;
  right?: boolean;
}> = ({ children, className, right }) => {
  return (
    <div className={cn(css.card, className, { [css.cardRight]: right })}>
      {children}
    </div>
  );
};
