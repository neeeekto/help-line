const rootMain = require('../../../.storybook/main');
const { mergeConfig } = require('vite');
import ViteTsConfigPathsPlugin from 'vite-tsconfig-paths';
export default {
  ...rootMain,

  core: { ...rootMain.core, builder: '@storybook/builder-vite' },
  features: {
    interactionsDebugger: true,
  },
  stories: ['../src/**/*.stories.mdx', '../src/**/*.stories.@(js|jsx|ts|tsx)'],
  addons: [
    '@storybook/addon-essentials',
    ...rootMain.addons,
    '@nrwl/react/plugins/storybook',
    '@storybook/addon-interactions',
  ],
  /*webpackFinal: async (config, { configType }) => {
    // apply any global webpack configs that might have been specified in .storybook/main.js
    if (rootMain.webpackFinal) {
      config = await rootMain.webpackFinal(config, { configType });
    }

    // add your own webpack tweaks if needed

    return config;
  },*/
  async viteFinal(config: any) {
    // Merge custom configuration into the default config
    return mergeConfig(config, {
      // Use the same "resolve" configuration as your app
      plugins: [
        ViteTsConfigPathsPlugin({
          root: '../../',
          projects: ['tsconfig.base.json'],
        }),
      ],
      define: {
        global: 'window',
      },
    });
  },
};
