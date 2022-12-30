import { ValueAccessor, ResourceType } from './types';
import {
  Component,
  Context,
  Template,
  TemplateBase,
} from '@help-line/entities/admin/api';

const tryParseJson = (val?: string, fallback: any = '') => {
  try {
    return JSON.parse(val || '');
  } catch (e) {
    return fallback;
  }
};

export const makeTemplateOrComponentContentValueAccessor = (): ValueAccessor<
  Template | Component
> => ({
  field: 'content',
  set: (val) => ({ content: val }),
  get: (rsc, current) => current?.content || rsc?.content || '',
});

export const makeGroupValueAccessor = (): ValueAccessor<TemplateBase> => ({
  field: 'group',
  set: (val) => ({ group: val }),
  get: (rsc, current) => current?.group || rsc?.group || '',
});

export const makeTemplatePropsValueAccessor = (): ValueAccessor<Template> => ({
  field: 'props',
  set: (val) => ({
    props: tryParseJson(val, {}),
  }),
  get: (rsc, current) => {
    return JSON.stringify(current?.props || rsc?.props || {}, null, 2);
  },
});

export const makeContextDataValueAccessor = (): ValueAccessor<Context> => ({
  field: 'data',
  set: (val) => ({ data: tryParseJson(val, {}) }),
  get: (rsc, current) =>
    JSON.stringify(current?.data || rsc?.data || {}, null, 2),
});

export const makeContextAliasValueAccessor = (): ValueAccessor<Context> => ({
  field: 'alias',
  set: (val) => ({ alias: val }),
  get: (rsc, current) => current?.alias || rsc?.alias || '',
});

export const makeContextExtendValueAccessor = (): ValueAccessor<Context> => ({
  field: 'extend',
  set: (val) => ({ extend: val }),
  get: (rsc, current) => current?.extend || rsc?.extend || '',
});

export const makeValueAccessorByResource = (type: ResourceType) => {
  switch (type) {
    case ResourceType.Context:
      return makeContextDataValueAccessor();
    case ResourceType.Template:
    case ResourceType.Component:
      return makeTemplateOrComponentContentValueAccessor();
  }
};
