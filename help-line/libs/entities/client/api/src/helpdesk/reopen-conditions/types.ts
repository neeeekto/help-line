export interface ReopenConditionData {
  name: string;
  minimalScore: number;
  mustSolved: boolean;
}

export interface ReopenCondition extends ReopenConditionData {
  id: string;
  enabled: boolean;
}
