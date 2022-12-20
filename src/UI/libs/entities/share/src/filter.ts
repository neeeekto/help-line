import { WithType } from './common.types';

export interface WithPath {
  path: string[];
}

export interface ConstantFilterValue extends WithType<'ConstantFilterValue'> {
  value: unknown;
}

export interface ContextFilterValue
  extends WithPath,
    WithType<'ContextFilterValue'> {}

export type FilterValue = ConstantFilterValue | ContextFilterValue;

export interface InFilter extends WithPath, WithType<'InFilter'> {
  values: FilterValue[];
}

export interface ContainsFilter extends WithPath, WithType<'ContainsFilter'> {
  values: FilterValue[];
}

export interface ElementMathFilter
  extends WithPath,
    WithType<'ElementMathFilter'> {
  itemFilter: Filter;
}

export enum GroupFilterOperators {
  And = 'And',
  Or = 'Or',
}
export interface GroupFilter extends WithType<'GroupFilter'> {
  operation: GroupFilterOperators;
  filters: Filter[];
}

export interface NotFilter extends WithType<'NotFilter'> {
  filter: Filter;
}

export interface OfTypeFilter extends WithType<'OfTypeFilter'> {
  type: string;
  filter?: Filter;
}

export enum FieldFilterOperators {
  Equal = 'Equal',
  NotEqual = 'NotEqual',
  Less = 'Less',
  LessOrEqual = 'LessOrEqual',
  Great = 'Great',
  GreatOrEqual = 'GreatOrEqual',
}
export interface ValueFilter extends WithPath, WithType<'ValueFilter'> {
  operator: FieldFilterOperators;
  value: FilterValue;
}

export type Filter =
  | InFilter
  | ContainsFilter
  | ElementMathFilter
  | GroupFilter
  | NotFilter
  | OfTypeFilter
  | ValueFilter;

export interface Sort extends WithPath {
  asc: boolean;
}
