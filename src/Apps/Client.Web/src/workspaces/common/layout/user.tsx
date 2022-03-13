import React from "react";
import { Avatar, Button } from "antd";
import {UserOutlined, LogoutOutlined, CrownOutlined} from "@ant-design/icons";
import css from "./layout.module.scss";
import { spacingCss, textCss } from "@shared/styles";
import cn from "classnames";
import { observer } from "mobx-react-lite";
import { useAuthStore$ } from "@core/auth";

export const User: React.FC = observer(() => {
  const authStore = useAuthStore$();
  const profile = authStore.profile.get();
  return (
    <div className={css.user}>
      <Avatar icon={profile?.isAdmin ? <CrownOutlined /> : <UserOutlined />} src={profile?.photo} />
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
        onClick={authStore.logoutGlobal}
      />
    </div>
  );
});
