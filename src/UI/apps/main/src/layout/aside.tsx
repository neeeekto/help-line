import React, { useCallback } from 'react';
import { useLogoutAction } from '@help-line/modules/auth';
import { Logo } from './logo';
import { Button, Tooltip } from 'antd';
import { OPEN_HELP_DELAY } from '@help-line/modules/application';
import { LogoutOutlined } from '@ant-design/icons';
import { NavigationMenu } from './navigation-menu';
import css from './layout.module.scss';

export const Aside: React.FC<{
  collapsed: boolean;
}> = ({ collapsed }) => {
  const logoutAction = useLogoutAction();
  const onLogout = useCallback(() => logoutAction.mutate(), [logoutAction]);

  return (
    <>
      <Logo collapsed={collapsed}>
        <Tooltip
          placement={'right'}
          title="Logout"
          mouseEnterDelay={OPEN_HELP_DELAY}
        >
          <Button type="link" size={'small'} onClick={onLogout}>
            <LogoutOutlined />
          </Button>
        </Tooltip>
      </Logo>
      <NavigationMenu className={css.menu} children={collapsed} />
    </>
  );
};
