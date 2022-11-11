export interface Operator {
  id: string;
  favorite: string[]; // TicketIds
  roles: Record<string, string[]>;
}

export interface OperatorView {
  id: string;
  firstName: string;
  lastName: string;
  photo: string;
  projects: string[];
  active: boolean;
}

export interface OperatorRoleData {
  title: string;
}

export interface OperatorRole {
  id: string;
  data: OperatorRoleData;
}
