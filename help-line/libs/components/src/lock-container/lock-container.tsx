import css from './lock-container.module.scss';
import React, { PropsWithChildren } from 'react';
import { FullPageContainer } from '../full-page-container';
import { Spin } from 'antd';

type LockContainerProps = PropsWithChildren<{
  text?: React.ReactNode;
}>;

export const LockContainer: React.FC<LockContainerProps> = ({
  text,
  children,
}) => {
  return (
    <FullPageContainer useCenterPlacement className={css.root}>
      {children ? <>{children}</> : <Spin tip={text} />}
    </FullPageContainer>
  );
};
