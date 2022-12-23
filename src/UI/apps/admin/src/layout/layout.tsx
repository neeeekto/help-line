import React, { PropsWithChildren, useCallback, useState } from 'react';
import { Layout } from 'antd';
import css from './layout.module.scss';
import { Content } from './content';

import { Aside } from './aside';

export const LayoutRoot: React.FC<PropsWithChildren> = ({ children }) => {
  const [collapsed, changeCollapse] = useState(false);

  return (
    <Layout className={css.box}>
      <Layout.Sider
        collapsible
        collapsed={collapsed}
        width={220}
        onCollapse={changeCollapse}
      >
        <Aside collapsed={collapsed} />
      </Layout.Sider>
      <Layout>
        <Content>{children}</Content>
      </Layout>
    </Layout>
  );
};
