import React, { PropsWithChildren } from 'react';
import { Layout } from 'antd';
import cl from './layout.module.scss';

export const Content: React.FC<PropsWithChildren> = ({ children }) => {
  return <Layout.Content className={cl.content}>{children}</Layout.Content>;
};
