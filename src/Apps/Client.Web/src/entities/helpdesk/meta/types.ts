import { WithType } from '@entities/common';

export enum Primitives {
  Number = 'Number',
  Boolean = 'Boolean',
  String = 'String',
  Date = 'Date',
}
export interface PrimitiveFieldType extends WithType<'PrimitiveDescriptionFieldType'> {
  type: Primitives;
}

export interface ArrayFieldType extends WithType<'ArrayDescriptionFieldType'> {
  itemType: FieldType;
}

export interface DictionaryFieldType extends WithType<'DictionaryDescriptionFieldType'> {
  keyType: FieldType;
  itemType: FieldType;
}

export interface ClassFieldType extends WithType<'ClassDescriptionFieldType'> {
  type: string;
}

export type FieldType = PrimitiveFieldType | ArrayFieldType | DictionaryFieldType | ClassFieldType;

export interface FieldDescription {
  path: string[];
  name?: string;
  description?: string;
  type: FieldType;
  optional: true;
}

export interface TypeDescription {
  key: string;
  title: string;
  description: string;
  fields: FieldDescription[];
  children: string[];
}

export interface EnumDescription {
  key: string;
  values: Record<string, number>;
}

export interface Description {
  root: string;
  types: TypeDescription[];
  enums: EnumDescription[];
}
