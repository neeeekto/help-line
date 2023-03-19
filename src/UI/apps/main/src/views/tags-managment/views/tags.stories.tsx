import { ComponentMeta } from '@storybook/react';
import { makeStoryFactory } from '../../../../../../libs/dev/storybook/src';
import React, { ComponentProps } from 'react';
import { MswHandlers } from '../../../../../../libs/dev/http-stubs/src';

import { Tags } from './tags';
import {
  helpdeskTagsClientStubApi,
  HelpdeskTagsClientStubs,
} from '../../../../../../libs/entities/client/stubs/src';

export default {
  component: Tags,
  title: 'views/Tags',
} as ComponentMeta<typeof Tags>;

const factory = makeStoryFactory((args: ComponentProps<typeof Tags>) => (
  <Tags {...args} projectId={'test'} />
));

export const Primary = factory.create({
  parameters: {
    msw: {
      handlers: [
        helpdeskTagsClientStubApi
          .get({ projectId: '*' })
          .handle(
            MswHandlers.success(
              new Array(10).fill('').map(HelpdeskTagsClientStubs.createTag)
            )
          ),
        helpdeskTagsClientStubApi
          .save({ projectId: '*', id: '*' })
          .handle(MswHandlers.success(null, { delay: 500 })),
        helpdeskTagsClientStubApi
          .saveMany({ projectId: '*' })
          .handle(MswHandlers.success(null, { delay: 500 })),
        helpdeskTagsClientStubApi
          .delete({ projectId: '*', id: '*' })
          .handle(MswHandlers.success(null, { delay: 500 })),
      ],
    },
  },
});
