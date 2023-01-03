import React, { PropsWithChildren } from 'react';
import { ConfigProvider, theme } from 'antd';

export const ThemeProvider: React.FC<PropsWithChildren> = ({ children }) => {
  return (
    <ConfigProvider
      theme={{
        algorithm: [theme.defaultAlgorithm],
        components: {
          Layout: {
            colorBgHeader: '#373d42',
          },
          Menu: {
            colorItemBg: '#373d42',
            colorSubItemBg: '#2e3338',
          },
        },
        token: {
          colorPrimary: '#19aa8d',
        },
      }}
    >
      {children}
    </ConfigProvider>
  );
};
