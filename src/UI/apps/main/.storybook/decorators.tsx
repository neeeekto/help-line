import React from 'react';
import { ThemeProvider } from '../src/styles/theme-provider';

export function globalStylesDecorator(story: React.FC) {
  return <ThemeProvider>{story()}</ThemeProvider>;
}
