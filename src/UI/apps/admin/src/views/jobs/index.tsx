import React from "react";
import { FullPageContainer } from "@shared/components/full-page-container";
import { JobsRoot } from "./jobs.root";
import { boxCss } from "@shared/styles";
import cn from "classnames";

const Jobs: React.FC = () => {
  return (
    <FullPageContainer className={cn(boxCss.flex, boxCss.flexColumn)}>
      <JobsRoot />
    </FullPageContainer>
  );
};

export default Jobs;
