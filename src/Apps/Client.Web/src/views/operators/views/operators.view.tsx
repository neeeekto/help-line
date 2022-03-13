import React from "react";
import {
  useOperatorsQuery,
  useOperatorsViewQuery,
} from "@entities/helpdesk/operators";
import { FullPageContainer } from "@shared/components/full-page-container";
import { Spin } from "antd";

export const OperatorsView: React.FC = () => {
  const operatorsQuery = useOperatorsQuery();

  if (operatorsQuery.isLoading && !operatorsQuery.isFetched) {
    return (
      <FullPageContainer useCenterPlacement>
        <Spin />
      </FullPageContainer>
    );
  }

  return <FullPageContainer>Operators</FullPageContainer>;
};
