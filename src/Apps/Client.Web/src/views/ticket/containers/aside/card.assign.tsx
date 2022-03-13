import React from "react";
import { PropsWithClassName } from "@shared/react.types";
import css from "./aside.module.scss";
import cn from "classnames";

export const CardAssign: React.FC<PropsWithClassName> = ({
  className,
  children,
}) => <div className={cn(css.box, className)}>{children}</div>;
