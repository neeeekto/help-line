import React, { PropsWithChildren } from 'react';
import { Card, theme } from 'antd';
import css from './aside.module.scss';
import cn from 'classnames';

export const AsideCardItem = ({
  className,
  children,
}: PropsWithChildren<{ className?: string }>) => {
  return <Card className={css.box}>{children}</Card>;
};
