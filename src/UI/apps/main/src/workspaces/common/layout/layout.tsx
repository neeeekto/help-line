import React, { PropsWithChildren, useState } from 'react';
import { Layout } from 'antd';
import cl from './layout.module.scss';
import { Content } from './content';
import { Logo } from './logo';
import { User } from './user';
import { NavigationMenu } from './navigation-menu';
import { ProjectSelector } from './footer';
import { useProjectIdParam } from '../../../hooks/router.hooks';
import cn from 'classnames';
import { boxCss } from '@help-line/style-utils';

export const LayoutRoot = ({ children }: PropsWithChildren) => {
  const [collapsed] = useState(false);
  const projectId = useProjectIdParam();

  return (
    <Layout className={cl.box}>
      <Layout.Sider
        trigger={null}
        collapsible
        collapsed={collapsed}
        width={220}
        theme={'dark'}
      >
        <div className={cn(boxCss.flex, boxCss.flexColumn, boxCss.fullHeight)}>
          <Logo />
          <User />
          <NavigationMenu projectId={projectId} className={cl.menu} />
          {!!projectId && <ProjectSelector projectId={projectId} />}
        </div>
      </Layout.Sider>
      <Layout className="site-layout">
        <Content>{children}</Content>
      </Layout>
    </Layout>
  );
};
