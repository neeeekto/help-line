import { WithType } from "@entities/common";

export interface Migration {
  name: string;
  description: string;
  params?: string;
  children: string[];
  parents: string[];
  isManual: boolean;
  applied: boolean;
  statuses: MigrationStatus[];
}

export interface MigrationStatusBase<T> extends WithType<T> {
  dateTime: string; //DateTime
}

export interface MigrationInQueueStatus
  extends MigrationStatusBase<"MigrationInQueueStatus"> {}

export interface MigrationExecutingStatus
  extends MigrationStatusBase<"MigrationExecutingStatus"> {}

export interface MigrationRollbackStatus
  extends MigrationStatusBase<"MigrationRollbackStatus"> {}

export interface MigrationRollbackSuccessStatus
  extends MigrationStatusBase<"MigrationRollbackSuccessStatus"> {}

export interface MigrationAppliedStatus
  extends MigrationStatusBase<"MigrationAppliedStatus"> {}

export interface MigrationAppliedAndSavedStatus
  extends MigrationStatusBase<"MigrationAppliedAndSavedStatus"> {}

export interface MigrationErrorStatus
  extends MigrationStatusBase<"MigrationErrorStatus"> {
  exception: any;
}

export interface MigrationRollbackErrorStatus
  extends MigrationStatusBase<"MigrationRollbackErrorStatus"> {
  exception: any;
}

export type MigrationStatus =
  | MigrationExecutingStatus
  | MigrationErrorStatus
  | MigrationRollbackErrorStatus
  | MigrationAppliedAndSavedStatus
  | MigrationAppliedStatus
  | MigrationInQueueStatus
  | MigrationRollbackStatus
  | MigrationRollbackSuccessStatus;
