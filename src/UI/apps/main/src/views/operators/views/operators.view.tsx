import React from 'react';

import { Spin } from 'antd';
import { useOperatorsQuery } from '@help-line/entities/client/query';
import { FullPageContainer } from '@help-line/components';
import { Project } from '@help-line/entities/client/api';

export const OperatorsView = ({ projectId }: { projectId: Project['id'] }) => {
  const operatorsQuery = useOperatorsQuery(projectId);

  if (operatorsQuery.isLoading && !operatorsQuery.isFetched) {
    return (
      <FullPageContainer useCenterPlacement>
        <Spin />
      </FullPageContainer>
    );
  }

  return <FullPageContainer>Operators</FullPageContainer>;
};
