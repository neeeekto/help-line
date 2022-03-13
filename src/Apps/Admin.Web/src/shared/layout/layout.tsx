import React, { useCallback, useState } from "react";
import { Button, Layout } from "antd";
import css from "./layout.module.scss";
import { Content } from "./content";
import { Logo } from "./logo";
import { NavigationMenu } from "./navigation-menu";
import { LeftOutlined, LogoutOutlined, RightOutlined } from "@ant-design/icons";
import { textCss } from "@shared/styles";
import { observer } from "mobx-react-lite";
import { useAuthStore$ } from "@core/auth";

const Aside: React.FC<{
  collapsed: boolean;
  onChangeCollapse?: (value: boolean) => void;
}> = observer(({ collapsed, onChangeCollapse }) => {
  const toggle = useCallback(() => {
    if (onChangeCollapse) {
      onChangeCollapse(!collapsed);
    }
  }, [collapsed]);
  const authStore = useAuthStore$();

  return (
    <>
      <Logo collapsed={collapsed} />
      <NavigationMenu className={css.menu} children={collapsed} />
      <Button type="link" onClick={authStore.logoutGlobal}>
        Logout
        <LogoutOutlined />
      </Button>
      <button className={css.toggleCollapseBtn} onClick={toggle}>
        {collapsed ? <RightOutlined /> : <LeftOutlined />}
      </button>
    </>
  );
});

export const LayoutRoot: React.FC = ({ children }) => {
  const [collapsed, changeCollapse] = useState(false);

  return (
    <Layout className={css.box}>
      <Layout.Sider
        trigger={null}
        collapsible
        collapsed={collapsed}
        width={220}
      >
        <Aside collapsed={collapsed} onChangeCollapse={changeCollapse} />
      </Layout.Sider>
      <Layout>
        <Content>{children}</Content>
      </Layout>
    </Layout>
  );
};
