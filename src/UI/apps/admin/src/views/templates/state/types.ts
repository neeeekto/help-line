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

export interface EditCache<T extends TemplateBase = TemplateBase> {
  resource: Resource['id'];
  value: Partial<T>;
  hash: Resource['hash'];
  temp?: any;
}

export interface ValueAccessorSetResult<T> {
  update: Partial<T>;
  temp?: any;
}

export interface ValueAccessor<T extends TemplateBase = TemplateBase> {
  field?: string;
  get: (rsc?: T, current?: Partial<T>, temp?: any) => string;
  set: (
    val?: string,
    cache?: Partial<T>,
    current?: Partial<T>
  ) => ValueAccessorSetResult<T>;
  equal?: (currentValue: Partial<T>, newValue: Partial<T>) => boolean;
}

export interface EditTab<T extends TemplateBase = TemplateBase> {
  id: string;
  title?: string;
  resource: Resource['id'];
  language: string;
  value: ValueAccessor<T>;
  readonly?: boolean;
  breadcrumb: string[];
}
