import React from "react";
import cn from "classnames";
import { boxCss } from "@shared/styles";

export const FullPageContainer: React.FC<{
  className?: string;
  useCenterPlacement?: boolean;
}> = ({ className, children, useCenterPlacement }) => {
  return (
    <div
      className={cn(
        className,
        boxCss.fullWidth,
        boxCss.fullHeight,
        useCenterPlacement && [
          boxCss.flex,
          boxCss.justifyContentCenter,
          boxCss.alignItemsCenter,
        ]
      )}
    >
      {children}
    </div>
  );
};
