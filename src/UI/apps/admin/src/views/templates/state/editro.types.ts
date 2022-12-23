import { TemplateBase } from '@help-line/entities/admin/api';

export enum SourceType {
  Template = 'template',
  Context = 'context',
  Component = 'component',
}

export interface EditedItem<T extends TemplateBase = TemplateBase> {
  src: SourceType;
  id: string;
  original?: T;
  current: T;
}

export interface Opened<T extends TemplateBase = TemplateBase> {
  id: string;
  lang: string;
  src: SourceType;
  field: string;
  active: boolean;
  value?: string; // Optional value
}
