export interface RoleData {
  name: string;
  permissions: string[];
}

export interface Role extends RoleData {
  readonly id: string;
}
