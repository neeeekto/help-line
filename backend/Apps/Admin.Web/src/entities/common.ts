export type LanguageDictionary<T = string> = Record<string, T>;

export interface WithType<T> {
  $type: T;
}

export interface KeyValue<TKey = any, TValue = any> {
  key: TKey;
  value: TValue;
}
