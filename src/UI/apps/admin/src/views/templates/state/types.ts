import { TemplateBase } from '@help-line/entities/admin/api';

export enum ResourceType {
  Template = 'template',
  Context = 'context',
  Component = 'component',
}

export interface Resource<T extends TemplateBase = TemplateBase> {
  id: string;
  type: ResourceType;
  data: T;
  hash: any;
  isNew?: boolean;
}

export interface EditCache {
  resource: Resource['id'];
  value: Partial<TemplateBase>;
  hash: Resource['hash'];
}

export interface ValueAccessor<T extends TemplateBase = TemplateBase> {
  field?: string;
  get: (rsc?: T, current?: Partial<T>) => string;
  set: (val?: string) => Partial<T>;
}

export interface EditTab {
  id: string;
  resource: Resource['id'];
  language: string;
  value: ValueAccessor;
  readonly?: boolean;
  breadcrumb: string[];
}
