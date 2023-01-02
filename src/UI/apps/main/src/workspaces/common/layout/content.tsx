import React, { PropsWithChildren } from 'react';
import { Layout } from 'antd';
import cl from './layout.module.scss';

export const Content = ({ children }: PropsWithChildren) => {
  return <Layout.Content className={cl.content}>{children}</Layout.Content>;
};
