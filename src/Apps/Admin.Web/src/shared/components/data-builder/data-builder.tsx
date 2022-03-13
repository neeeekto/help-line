import React, { useCallback, useState } from "react";
import { Description } from "@entities/meta.types";
import { ObjectBuilder } from "./object-builder";
import { BuilderProps } from "./data-builder.type";
import set from "lodash/set";

export const DataBuilder: React.FC<{
  description: Description;
  value?: any;
  onChange?: (value: any) => void;
  initValueFactory?: () => any;
  className?: string;
}> = ({ value, description, onChange, initValueFactory, className }) => {
  const [obj, setObject] = useState(
    value || (initValueFactory ? initValueFactory() : {})
  );
  const update: BuilderProps["setValue"] = useCallback(
    (value: any, path: string[]) => {
      set(obj, path, value);
      const next = { ...obj };
      onChange && onChange(next);
      setObject(next);
    },
    [obj]
  );
  return (
    <div className={className}>
      <ObjectBuilder
        type={description.root}
        description={description}
        value={obj}
        setValue={update}
      />
    </div>
  );
};
