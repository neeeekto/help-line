import React, { useCallback } from 'react';
import { Logo } from './logo';
import { Button, Tooltip } from 'antd';
import { OPEN_HELP_DELAY } from '@help-line/modules/application';
import { LogoutOutlined } from '@ant-design/icons';
import { NavigationMenu } from './navigation-menu';
import css from './layout.module.scss';
import { observer } from 'mobx-react-lite';
import { useAuthStore$ } from '@help-line/modules/auth';

interface Props {
  collapsed: boolean;
}

export const Aside = observer(({ collapsed }: Props) => {
  const authStore$ = useAuthStore$();
  const onLogout = useCallback(() => authStore$.logout(), [authStore$]);

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
});
