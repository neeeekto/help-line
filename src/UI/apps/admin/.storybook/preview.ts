import '../src/styles/global';
import { diDecorator, globalStylesDecorator } from './decorators';
import { initialize, mswDecorator } from 'msw-storybook-addon';
import { setupI18n } from '@help-line/modules/application';
import * as jest from 'jest-mock';
window.jest = jest;
setupI18n();
initialize({ onUnhandledRequest: 'bypass' });

export const decorators = [mswDecorator, globalStylesDecorator, diDecorator];

export const parameters = {
  actions: { argTypesRegex: '^on[A-Z].*' },
  controls: {
    matchers: {
      color: /(background|color)$/i,
      date: /Date$/,
    },
  },
};
