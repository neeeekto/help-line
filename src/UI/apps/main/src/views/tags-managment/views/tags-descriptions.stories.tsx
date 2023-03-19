import { ComponentMeta } from '@storybook/react';
import { makeStoryFactory } from '../../../../../../libs/dev/storybook/src';
import React, { ComponentProps } from 'react';
import { MswHandlers } from '../../../../../../libs/dev/http-stubs/src';
import { TagsDescriptions } from './tags-descriptions';
import {
  HelpdeskTagsClientStubs,
  helpdeskTagsDescClientStubApi,
} from '../../../../../../libs/entities/client/stubs/src';

export default {
  component: TagsDescriptions,
  title: 'views/TagsDescriptions',
} as ComponentMeta<typeof TagsDescriptions>;

const factory = makeStoryFactory(
  (args: ComponentProps<typeof TagsDescriptions>) => (
    <TagsDescriptions {...args} projectId={'test'} />
  )
);

export const Primary = factory.create({
  parameters: {
    msw: {
      handlers: [
        helpdeskTagsDescClientStubApi
          .get({ projectId: '*' })
          .handle(
            MswHandlers.success(
              new Array(10).fill('').map(HelpdeskTagsClientStubs.createTagDesc)
            )
          ),
        helpdeskTagsDescClientStubApi
          .save({ projectId: '*', id: '*' })
          .handle(MswHandlers.success(null, { delay: 500 })),
        helpdeskTagsDescClientStubApi
          .delete({ projectId: '*', id: '*' })
          .handle(MswHandlers.success(null, { delay: 500 })),
      ],
    },
  },
});
