import React, { PropsWithChildren } from 'react';
import css from './discussion.module.scss';
import cn from 'classnames';

export const TicketDiscussionCard = ({
  children,
  className,
  right,
}: PropsWithChildren<{
  className?: string;
  right?: boolean;
}>) => {
  return (
    <div className={cn(css.card, className, { [css.cardRight]: right })}>
      {children}
    </div>
  );
};
