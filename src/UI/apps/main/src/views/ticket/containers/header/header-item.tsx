import React, { PropsWithChildren } from 'react';
import css from './header.module.scss';
import cn from 'classnames';
import { spacingCss } from '@help-line/style-utils';

export const HeaderItem = ({
  children,
  className,
  title,
}: PropsWithChildren<{
  title: React.ReactElement | string;
  className?: string;
}>) => (
  <span className={cn(css.headerItem, className)}>
    {title}:<b className={spacingCss.marginLeftXs}>{children}</b>
  </span>
);
