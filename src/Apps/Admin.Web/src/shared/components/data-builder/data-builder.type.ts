import { Description, FieldDescription } from "@entities/meta.types";

export interface BuilderProps {
  description: Description;
  parent?: FieldDescription[];
  value: any;
  setValue?: (value: any, path: string[]) => void;
}

export interface FieldBuilderProps extends BuilderProps {
  field: FieldDescription;
}
