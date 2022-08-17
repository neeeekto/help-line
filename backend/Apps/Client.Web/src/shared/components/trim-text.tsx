import React from "react";
import cn from "classnames";
import {textCss} from "@shared/styles";

export const TrimText: React.FC<{maxWith: string}> = ({children, maxWith}) => {
    return <span className={cn(textCss.truncate)} style={{maxWidth: maxWith}}>{children}</span>
}
