import React from "react";
import { FieldBuilderProps } from "@shared/components/data-builder/data-builder.type";
import { ClassFieldType } from "@entities/meta.types";
import { ObjectBuilder } from "@shared/components/data-builder/object-builder";
import { FieldName } from "@shared/components/data-builder/field-name";
import { boxCss, spacingCss } from "@shared/styles";
import cn from "classnames";
import { Typography } from "antd";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export const ClassField: React.FC<
  FieldBuilderProps & {
    type: ClassFieldType;
  }
> = React.memo((props) => {
  return (
    <div>
      <div
        className={cn(
          spacingCss.marginBottomSm,
          boxCss.flex,
          boxCss.justifyContentSpaceBetween
        )}
      >
        <FieldName field={props.field} icon={<FontAwesomeIcon icon="cube" />} />
        <Typography.Text>{props.field.description}</Typography.Text>
      </div>
      <ObjectBuilder
        description={props.description}
        type={props.type.type}
        value={props.value}
        setValue={props.setValue}
        parent={[...(props.parent || []), props.field]}
      />
    </div>
  );
});
