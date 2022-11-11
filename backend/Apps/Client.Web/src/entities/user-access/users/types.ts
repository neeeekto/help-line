interface UserBase {
  email: string;
  info: UserInfo;
}

export interface UserInfo {
  firstName: string;
  lastName: string;
  photo: string;
  language: string;
}

export interface UserRoles {
  globalRoles: string[];
  projectRoles: {
    [projectKey: string]: string[];
  };
  projects: string[];
}

export interface UserData extends UserBase, UserRoles {
  permissions: string[];
}

export enum UserStatus {
  Deleted = "Deleted",
  Active = "Active",
}

export interface User extends UserBase {
  id: string;
  status: UserStatus;
}
