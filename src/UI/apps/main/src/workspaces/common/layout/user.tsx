import React from 'react';
import { Avatar, Button } from 'antd';
import { UserOutlined, LogoutOutlined, CrownOutlined } from '@ant-design/icons';
import css from './layout.module.scss';
import { spacingCss, textCss } from '@help-line/style-utils';
import cn from 'classnames';
import { useAuthProfile, useLogoutAction } from '@help-line/modules/auth';

export const User = () => {
  const logout = useLogoutAction();
  const profile = useAuthProfile();
  return (
    <div className={css.user}>
      <Avatar
        icon={profile?.isAdmin ? <CrownOutlined /> : <UserOutlined />}
        src={profile?.photo}
      />
      <h4
        className={cn(
          spacingCss.marginLeftSm,
          spacingCss.marginBottomNone,
          spacingCss.marginRightAuto,
          textCss.white
        )}
      >
        {profile?.firstName} {profile?.lastName}
      </h4>
      <Button
        className={spacingCss.marginLeftXs}
        type="link"
        icon={<LogoutOutlined />}
        onClick={() => logout.mutate()}
      />
    </div>
  );
};
