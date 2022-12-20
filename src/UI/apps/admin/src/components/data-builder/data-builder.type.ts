import { Description, FieldDescription } from '@help-line/entities/share';

export interface BuilderProps {
  description: Description;
  parent?: FieldDescription[];
  value: any;
  setValue?: (value: any, path: string[]) => void;
}

export interface FieldBuilderProps extends BuilderProps {
  field: FieldDescription;
}
