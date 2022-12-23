import { FieldDescription } from "@entities/meta.types";
import { useMemo } from "react";
import get from "lodash/get";
import flatten from "lodash/flatten";
import lowerFirst from "lodash/lowerFirst";

export const usePath = (...fields: FieldDescription[]) => {
  return useMemo(
    () =>
      flatten(
        fields.filter((x) => x).map((x) => x.path.map((p) => lowerFirst(p)))
      ),
    [fields]
  );
};

export const useValue = (obj: any, ...fields: FieldDescription[]) => {
  const path = usePath(...fields);
  const value = useMemo(() => get(obj, path), [obj, path]);
  return [value, path];
};
