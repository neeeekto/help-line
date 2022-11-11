import React, { useCallback, useMemo } from "react";
import { FieldBuilderProps } from "@shared/components/data-builder/data-builder.type";
import { EnumFieldType } from "@entities/meta.types";
import { Select } from "antd";
import {
  usePath,
  useValue,
} from "@shared/components/data-builder/data-builder.hooks";
import { RowFieldView } from "@shared/components/data-builder/row-field-view";

export const EnumField: React.FC<
  FieldBuilderProps & {
    type: EnumFieldType;
  }
> = (props) => {
  const path = usePath(...(props.parent || []), props.field);
  const enm = useMemo(() => {
    const enm = props.description.enums.find((x) => x.key === props.type.enum)!;
    return Object.keys(enm.values).map((x) => ({
      key: x,
      value: enm.values[x],
    }));
  }, [props.description, props.type]);
  const onChange = useCallback(
    (val: any) => {
      props.setValue && props.setValue(val, path);
    },
    [props.setValue, path]
  );
  return (
    <RowFieldView field={props.field}>
      <Select value={props.value} onSelect={onChange}>
        {enm.map((x) => (
          <Select.Option key={x.key} value={x.value}>
            {x.key}
          </Select.Option>
        ))}
      </Select>
    </RowFieldView>
  );
};
