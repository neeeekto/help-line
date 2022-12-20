import React from 'react';
import { ComponentStory } from '@storybook/react';

export const makeStoryFactory = <T>(render: React.FC<T>) => {
  return {
    create: (params: Partial<ComponentStory<React.FC<T>>>) => {
      const fn: any = render.bind({});
      Object.entries(params).forEach(([key, val]) => {
        fn[key] = val;
      });
      return fn;
    },
  };
};
