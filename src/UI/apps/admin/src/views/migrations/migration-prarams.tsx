import React, { useMemo, useState } from 'react';
import { useMigrationsParamsQuery } from '@help-line/entities/admin/query';
import { Spin } from 'antd';
import { DataBuilder } from '../../components/data-builder';
import { Migration } from '@help-line/entities/admin/api';

export const MigrationParamsBuilder: React.FC<{
  migration: Migration;
  params?: any;
  onChange?: (val: any) => void;
}> = ({ migration, params, onChange }) => {
  const paramsDesc = useMigrationsParamsQuery();

  const desc = (paramsDesc.data || {})[migration.params!];

  if (paramsDesc.isLoading) {
    return <Spin />;
  }

  return (
    <div>
      <DataBuilder
        description={desc}
        value={params}
        onChange={onChange}
      ></DataBuilder>
    </div>
  );
};
