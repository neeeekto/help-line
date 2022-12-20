import React from "react";
import { FullPageContainer } from "@shared/components/full-page-container";
import { Spin } from "antd";

export const PageSpin: React.FC<{ className?: string }> = ({ children }) => {
  return (
    <FullPageContainer useCenterPlacement>
      {children || <Spin />}
    </FullPageContainer>
  );
};
