import React from 'react';
import { MigrationsProvider } from './views/migrations';

export const AppRoutes: React.FC = () => {
  return <MigrationsProvider>routes</MigrationsProvider>;
};
