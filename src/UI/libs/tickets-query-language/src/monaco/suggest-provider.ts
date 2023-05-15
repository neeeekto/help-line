export interface ISuggestProvider {
  getLang(search?: string): Promise<string[]>;
  getOperators(search?: string): Promise<string[]>;
}
