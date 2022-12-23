import React, { useMemo, useState } from "react";
import { useMigrationsParamsQuery } from "@entities/migrations/queries";
import { Spin } from "antd";
import { DataBuilder } from "@shared/components/data-builder";
import { Migration } from "@entities/migrations";

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
