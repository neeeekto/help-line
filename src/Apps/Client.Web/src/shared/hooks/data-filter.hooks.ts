import { useMemo } from "react";

type DataFilterType = {
  <T, TF1>(
    source: T[],
    filters: [(item: T, value: TF1) => boolean],
    filtersValues: [TF1]
  ): T[];
  <T, TF1, TF2>(
    source: T[],
    filters: [
      (item: T, value: TF1) => boolean,
      (item: T, value: TF2) => boolean
    ],
    filtersValues: [TF1, TF2]
  ): T[];
  <T, TF1, TF2, TF3>(
    source: T[],
    filters: [
      (item: T, value: TF1) => boolean,
      (item: T, value: TF2) => boolean,
      (item: T, value: TF3) => boolean
    ],
    filtersValues: [TF1, TF2, TF3]
  ): T[];
  <T, TF1, TF2, TF3, TF4>(
    source: T[],
    filters: [
      (item: T, value: TF1) => boolean,
      (item: T, value: TF2) => boolean,
      (item: T, value: TF3) => boolean,
      (item: T, value: TF4) => boolean
    ],
    filtersValues: [TF1, TF2, TF3, TF4]
  ): T[];
  <T, TF1, TF2, TF3, TF4, TF5>(
    source: T[],
    filters: [
      (item: T, value: TF1) => boolean,
      (item: T, value: TF2) => boolean,
      (item: T, value: TF3) => boolean,
      (item: T, value: TF4) => boolean,
      (item: T, value: TF5) => boolean
    ],
    filtersValues: [TF1, TF2, TF3, TF4, TF5]
  ): T[];
  <T, TF1, TF2, TF3, TF4, TF5, TF6>(
    source: T[],
    filters: [
      (item: T, value: TF1) => boolean,
      (item: T, value: TF2) => boolean,
      (item: T, value: TF3) => boolean,
      (item: T, value: TF4) => boolean,
      (item: T, value: TF5) => boolean,
      (item: T, value: TF6) => boolean
    ],
    filtersValues: [TF1, TF2, TF3, TF4, TF5, TF6]
  ): T[];
  <T, TF1, TF2, TF3, TF4, TF5, TF6, TF7>(
    source: T[],
    filters: [
      (item: T, value: TF1) => boolean,
      (item: T, value: TF2) => boolean,
      (item: T, value: TF3) => boolean,
      (item: T, value: TF4) => boolean,
      (item: T, value: TF5) => boolean,
      (item: T, value: TF6) => boolean,
      (item: T, value: TF7) => boolean
    ],
    filtersValues: [TF1, TF2, TF3, TF4, TF5, TF6, TF7]
  ): T[];
  <T, TF1, TF2, TF3, TF4, TF5, TF6, TF7, TF8>(
    source: T[],
    filters: [
      (item: T, value: TF1) => boolean,
      (item: T, value: TF2) => boolean,
      (item: T, value: TF3) => boolean,
      (item: T, value: TF4) => boolean,
      (item: T, value: TF5) => boolean,
      (item: T, value: TF6) => boolean,
      (item: T, value: TF7) => boolean,
      (item: T, value: TF8) => boolean
    ],
    filtersValues: [TF1, TF2, TF3, TF4, TF5, TF6, TF7, TF8]
  ): T[];
  <T, TF1, TF2, TF3, TF4, TF5, TF6, TF7, TF8, TF9>(
    source: T[],
    filters: [
      (item: T, value: TF1) => boolean,
      (item: T, value: TF2) => boolean,
      (item: T, value: TF3) => boolean,
      (item: T, value: TF4) => boolean,
      (item: T, value: TF5) => boolean,
      (item: T, value: TF6) => boolean,
      (item: T, value: TF7) => boolean,
      (item: T, value: TF8) => boolean,
      (item: T, value: TF9) => boolean
    ],
    filtersValues: [TF1, TF2, TF3, TF4, TF5, TF6, TF7, TF8, TF9]
  ): T[];
  <T>(
    source: T[],
    filters: Array<(item: T, value: any) => boolean>,
    filtersValues: any[]
  ): T[];
};

export const useDataFilter: DataFilterType = <T>(
  source: T[],
  filters: Array<(item: T, value: any) => boolean>,
  filtersValues: any[]
) => {
  return useMemo(() => {
    return source.filter((item) => {
      let result = true;
      for (let i = 0; i < filters.length; i++) {
        if (!result) break;

        const filter = filters[i];
        const value = filtersValues[i];
        result = result && filter(item, value);
      }
      return result;
    });
  }, [source, ...filtersValues]);
};
