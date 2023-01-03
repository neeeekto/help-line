import { ComponentMeta } from '@storybook/react';
import {
  makeStoryFactory,
  StorybookWrapper,
} from '../../../../../../libs/dev/storybook/src';
import React, { ComponentProps } from 'react';
import {
  helpdeskProjectClientStubApi,
  HelpdeskProjectsClientStubs,
} from '../../../../../../libs/entities/client/stubs/src';
import { MswHandlers } from '../../../../../../libs/dev/http-stubs/src';

import { LayoutRoot } from './index';
import { Route, Router, Routes, useLocation } from 'react-router-dom';
import { PROJECT_KEY } from '../../../core/router.constants';

export default {
  component: LayoutRoot,
  title: 'layout/LayoutRoot',
} as ComponentMeta<typeof LayoutRoot>;

const factory = makeStoryFactory((args: ComponentProps<typeof LayoutRoot>) => (
  <StorybookWrapper initialRoutes={['/test/']}>
    <Routes>
      <Route
        element={<LayoutRoot {...args}>Test</LayoutRoot>}
        path={`/:${PROJECT_KEY}`}
      ></Route>
      <Route element={<LayoutRoot {...args}>Test</LayoutRoot>} path="*"></Route>
    </Routes>
  </StorybookWrapper>
));

export const Primary = factory.create({
  parameters: {
    msw: {
      handlers: [
        helpdeskProjectClientStubApi
          .get()
          .handle(
            MswHandlers.success(
              new Array(10)
                .fill('')
                .map(HelpdeskProjectsClientStubs.createProject)
            )
          ),
      ],
    },
  },
});
