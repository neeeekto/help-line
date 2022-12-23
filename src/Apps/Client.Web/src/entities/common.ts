export type LanguageDictionary<T = string> = Record<string, T>;

export interface WithType<T> {
  $type: T;
}

export type Guid = string;
