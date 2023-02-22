import React from 'react';
import { ThemeProvider } from '../src/styles/theme-provider';
import { StorybookWrapper } from '@help-line/dev/storybook';
import { DiProvider } from '../src/di';

export function globalStylesDecorator(story: React.FC) {
  return <ThemeProvider>{story()}</ThemeProvider>;
}

export function diDecorator(story: React.FC) {
  return (
    <StorybookWrapper>
      <DiProvider>{story()}</DiProvider>
    </StorybookWrapper>
  );
}
