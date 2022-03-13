import React from "react";
import { FieldDescription } from "@entities/meta.types";
import { boxCss, spacingCss } from "@shared/styles";
import { Typography } from "antd";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import cn from "classnames";

export const FieldName: React.FC<{
  field: FieldDescription;
  icon?: React.ReactElement;
}> = ({ field, icon }) => (
  <Typography.Text
    className={cn(
      spacingCss.marginRightSm,
      boxCss.inlineFlex,
      boxCss.alignItemsCenter
    )}
  >
    {icon && (
      <Typography.Text
        type="secondary"
        className={spacingCss.marginRightSm}
        style={{ fontSize: "10px" }}
      >
        {icon}
      </Typography.Text>
    )}
    {field.name || field.path.join(".")}
  </Typography.Text>
);
