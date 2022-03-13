import css from "./init-view.module.scss";
import React from "react";
import { FullPageContainer } from "@shared/components/full-page-container";
import { Spin } from "antd";

export const InitView: React.FC<{ text?: string }> = ({ text, children }) => {
  return (
    <FullPageContainer useCenterPlacement className={css.bg}>
      {children ? <>{children}</> : <Spin tip={text} />}
    </FullPageContainer>
  );
};
