import React from "react";
import css from "./header.module.scss";
import { PropsWithClassName } from "@shared/react.types";
import cn from "classnames";
import { spacingCss } from "@shared/styles";

export const HeaderItem: React.FC<
  PropsWithClassName & { title: React.ReactElement | string }
> = ({ children, className, title }) => (
  <span className={cn(css.headerItem, className)}>
    {title}:<b className={spacingCss.marginLeftXs}>{children}</b>
  </span>
);
