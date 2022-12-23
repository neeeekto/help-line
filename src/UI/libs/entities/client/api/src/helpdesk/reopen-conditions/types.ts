import { GUID } from '@help-line/entities/share';

export interface ReopenConditionData {
  name: string;
  minimalScore: number;
  mustSolved: boolean;
}

export interface ReopenCondition extends ReopenConditionData {
  id: GUID;
  enabled: boolean;
}
