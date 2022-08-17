import React from "react";
import { boxCss } from "@shared/styles";
import { FieldBuilderProps } from "./data-builder.type";
import { PrimitiveField } from "./primitive-field";
import { EnumField } from "./enum-field";
import { ClassField } from "./class-field";
import { useValue } from "@shared/components/data-builder/data-builder.hooks";

export const FieldBuilder: React.FC<FieldBuilderProps> = React.memo((props) => {
  const [fieldValue, path] = useValue(props.value, props.field);
  switch (props.field.type.$type) {
    case "PrimitiveDescriptionFieldType":
      return (
        <PrimitiveField {...props} value={fieldValue} type={props.field.type} />
      );
    case "EnumDescriptionFieldType":
      return (
        <EnumField {...props} value={fieldValue} type={props.field.type} />
      );
    case "ArrayDescriptionFieldType":
      break;
    case "DictionaryDescriptionFieldType":
      break;
    case "ClassDescriptionFieldType":
      return (
        <ClassField {...props} value={fieldValue} type={props.field.type} />
      );
  }
  return <div className={boxCss.flex}>not implement...</div>;
});
