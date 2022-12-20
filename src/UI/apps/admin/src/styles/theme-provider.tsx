import React, { PropsWithChildren } from 'react';
import { ConfigProvider, theme } from 'antd';

export const ThemeProvider: React.FC<PropsWithChildren> = ({ children }) => {
  return (
    <ConfigProvider
      theme={{
        algorithm: [theme.defaultAlgorithm],
        token: {
          colorPrimary: '#1961aa',
        },
      }}
    >
      {children}
    </ConfigProvider>
  );
};
