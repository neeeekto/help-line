import {
  Component,
  Context,
  Template,
  TemplateBase,
} from '@help-line/entities/admin/api';
import { EditedItem, Opened, SourceType } from '../state/editro.types';

export const useTemplateItemValue = (opened: Opened, edit: EditedItem) => {
  const value = (edit.current as any)[opened.field];
  switch (opened.lang) {
    case 'json':
      return JSON.stringify(value, null, 2);
    default:
      return value;
  }
};

export const getMainFieldForSrc = (src: SourceType) => {
  switch (src) {
    case SourceType.Context:
      return 'data';
    case SourceType.Template:
    case SourceType.Component:
      return 'content';
    default:
      return '';
  }
};

export const useTemplateValueFactory = (opened: Opened) => {
  return (value: string) => {
    switch (opened.lang) {
      case 'json':
        try {
          return JSON.parse(value);
        } catch (e) {
          return {};
        }
      default:
        return value;
    }
  };
};

export const createTemplateItem = (id: string, type: string) => {
  const commonData = {
    id,
    group: '',
    updatedAt: new Date(Date.now()).toISOString(),
  } as TemplateBase;
  switch (type) {
    case SourceType.Template:
      return {
        ...commonData,
        props: {},
        content: '',
        contexts: [],
        name: '',
      } as Template;
    case SourceType.Context:
      return {
        ...commonData,
        data: {},
      } as Context;
    case SourceType.Component:
      return {
        ...commonData,
        content: '',
      } as Component;
  }
  return commonData;
};
