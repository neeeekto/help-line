import React, { PropsWithChildren } from 'react';
import css from './layout.module.scss';
import { LogoutOutlined } from '@ant-design/icons';
import { Button } from 'antd';

export const Logo: React.FC<PropsWithChildren<{ collapsed?: boolean }>> = ({
  collapsed,
  children,
}) => {
  return (
    <div className={css.logo}>
      <h3 className={css.logoText}>{collapsed ? 'HL' : 'Help Line Admin'}</h3>
      {children}
    </div>
  );
};
