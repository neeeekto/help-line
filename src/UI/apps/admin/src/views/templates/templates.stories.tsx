import { ComponentMeta } from '@storybook/react';
import {
  makeStoryFactory,
  StorybookWrapper,
} from '../../../../../libs/dev/storybook/src';
import { LayoutRoot } from '../../layout';
import React from 'react';
import { TemplatesView } from './templates';
import {
  adminHelpdeskStubApi,
  adminComponentsStubApi,
  adminContextsStubApi,
  adminTemplatesStubApi,
  AdminTemplateRendererStubs,
} from '../../../../../libs/entities/admin/stubs/src';
import { MswHandlers } from '../../../../../libs/dev/http-stubs/src';

export default {
  component: TemplatesView,
  title: 'views/Templates',
} as ComponentMeta<typeof TemplatesView>;

const factory = makeStoryFactory(() => (
  <LayoutRoot>
    <TemplatesView />
  </LayoutRoot>
));

export const Primary = factory.create({
  parameters: {
    msw: {
      handlers: [
        adminComponentsStubApi
          .delete({ id: '*' })
          .handle(MswHandlers.success(null, { delay: 500 })),
        adminTemplatesStubApi
          .delete({ id: '*' })
          .handle(MswHandlers.success(null, { delay: 500 })),
        adminContextsStubApi
          .delete({ id: '*' })
          .handle(MswHandlers.success(null, { delay: 500 })),
        adminComponentsStubApi
          .get()
          .handle(
            MswHandlers.success([
              AdminTemplateRendererStubs.createComponent(),
              AdminTemplateRendererStubs.createComponent(),
              AdminTemplateRendererStubs.createComponent(),
              AdminTemplateRendererStubs.createComponent(),
              AdminTemplateRendererStubs.createComponent(),
            ])
          ),
        adminContextsStubApi
          .get()
          .handle(
            MswHandlers.success([
              AdminTemplateRendererStubs.createContext(),
              AdminTemplateRendererStubs.createContext(),
              AdminTemplateRendererStubs.createContext(),
              AdminTemplateRendererStubs.createContext(),
              AdminTemplateRendererStubs.createContext(),
            ])
          ),
        adminTemplatesStubApi
          .get()
          .handle(
            MswHandlers.success([
              AdminTemplateRendererStubs.createTemplate(),
              AdminTemplateRendererStubs.createTemplate(),
              AdminTemplateRendererStubs.createTemplate(),
            ])
          ),
      ],
    },
  },
});
