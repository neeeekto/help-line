import React from 'react';
import { ThemeProvider } from '../src/styles/theme-provider';
import { StorybookWrapper } from '@help-line/dev/storybook';
import { setupAppDI } from '../src/di';
import { DiContainer } from '@help-line/modules/di';

const diContainer = setupAppDI({ apiUrl: '' });

export function globalStylesDecorator(story: React.FC) {
  return <ThemeProvider>{story()}</ThemeProvider>;
}

export function diDecorator(story: React.FC) {
  return (
    <StorybookWrapper>
      <DiContainer container={diContainer}>{story()}</DiContainer>
    </StorybookWrapper>
  );
}
