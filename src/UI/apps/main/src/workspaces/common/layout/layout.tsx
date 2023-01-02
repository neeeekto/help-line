import React, { useState } from "react";
import { Layout } from "antd";
import cl from "./layout.module.scss";
import { Content } from "./content";
import { Logo } from "@workspaces/common/layout/logo";
import { User } from "@workspaces/common/layout/user";
import { NavigationMenu } from "@workspaces/common/layout/navigation-menu";
import { Footer } from "@workspaces/common/layout/footer";

const Aside: React.FC = () => {
  return (
    <>
      <Logo />
      <User />
      <NavigationMenu className={cl.menu} />
      <Footer />
    </>
  );
};

export const LayoutRoot: React.FC = ({ children }) => {
  const [collapsed] = useState(false);

  return (
    <Layout className={cl.box}>
      <Layout.Sider
        trigger={null}
        collapsible
        collapsed={collapsed}
        width={220}
      >
        <Aside />
      </Layout.Sider>
      <Layout className="site-layout">
        <Content>{children}</Content>
      </Layout>
    </Layout>
  );
};
