import React from "react";
import { FieldDescription } from "@entities/meta.types";
import cn from "classnames";
import { boxCss, spacingCss } from "@shared/styles";
import { FieldName } from "@shared/components/data-builder/field-name";

export const RowFieldView: React.FC<{
  field: FieldDescription;
  icon?: React.ReactElement;
}> = ({ field, children, icon }) => (
  <div
    className={cn(
      boxCss.flex,
      boxCss.alignItemsCenter,
      spacingCss.marginBottomMd
    )}
  >
    <FieldName field={field} icon={icon} />
    {children}
  </div>
);
