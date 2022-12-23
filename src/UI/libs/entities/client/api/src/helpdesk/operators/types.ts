import { GUID } from '@help-line/entities/share';

export interface Operator {
  id: GUID;
  favorite: string[]; // TicketIds
  roles: Record<string, string[]>;
}

export interface OperatorRoleData {
  title: string;
}

export interface OperatorRole {
  id: string;
  data: OperatorRoleData;
}
