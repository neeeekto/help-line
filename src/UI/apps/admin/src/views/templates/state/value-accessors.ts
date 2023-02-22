import { ValueAccessor, ResourceType, ValueAccessorSetResult } from './types';
import {
  Component,
  Context,
  Template,
  TemplateBase,
} from '@help-line/entities/admin/api';
import isNil from 'lodash/isNil';
import isEqual from 'lodash/isEqual';

const tryParseJson = (val?: string, fallback: any = '') => {
  try {
    return { success: true, val: JSON.parse(val || '') };
  } catch (e) {
    return { success: false, val: fallback };
  }
};

const getValueOrReturnFallback = <T>(
  val: T | undefined | null,
  fallback: T
) => {
  return isNil(val) ? fallback : val;
};

const createUpdateResult = <T>(val: Partial<T>, temp?: any) =>
  ({ update: val, temp } as ValueAccessorSetResult<T>);

export const makeTemplateOrComponentContentValueAccessor = (): ValueAccessor<
  Template | Component
> => ({
  field: 'content',
  set: (val) => createUpdateResult({ content: val }),
  get: (rsc, current) =>
    getValueOrReturnFallback(current?.content, rsc?.content || ''),
});

export const makeGroupValueAccessor = (): ValueAccessor<TemplateBase> => ({
  field: 'group',
  set: (val) => createUpdateResult({ group: val }),
  get: (rsc, current) =>
    getValueOrReturnFallback(current?.group, rsc?.group || ''),
});

export const makeTemplatePropsValueAccessor = (): ValueAccessor<Template> => ({
  field: 'props',
  set: (strVal, cache, src) => {
    const { success, val } = tryParseJson(
      strVal,
      cache?.props || src?.props || {}
    );
    return createUpdateResult(
      {
        props: success ? val : cache?.props || src?.props,
      },
      strVal
    );
  },
  get: (rsc, current, temp) => {
    return temp || JSON.stringify(current?.props || rsc?.props || {}, null, 2);
  },
});

export const makeContextDataValueAccessor = (): ValueAccessor<Context> => ({
  field: 'data',
  equal: (currentValue, newValue) => isEqual(currentValue.data, newValue.data),
  set: (strVal, cache, src) => {
    const { success, val } = tryParseJson(strVal, cache?.data);
    return createUpdateResult(
      {
        data: success ? val : cache?.data || src?.data,
      },
      strVal
    );
  },
  get: (rsc, current, temp) =>
    temp || JSON.stringify(current?.data || rsc?.data || {}, null, 2),
});

export const makeContextAliasValueAccessor = (): ValueAccessor<Context> => ({
  field: 'alias',
  set: (val) => createUpdateResult({ alias: val }),
  get: (rsc, current) =>
    getValueOrReturnFallback(current?.alias, rsc?.alias || ''),
});

export const makeContextExtendValueAccessor = (): ValueAccessor<Context> => ({
  field: 'extend',
  set: (val) => createUpdateResult({ extend: val }),
  get: (rsc, current) =>
    getValueOrReturnFallback(current?.extend, rsc?.extend || ''),
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
