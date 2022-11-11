/* eslint-disable @typescript-eslint/no-empty-interface */
import { WithType } from '@help-line/api/share';

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
  extends MigrationStatusBase<'MigrationInQueueStatus'> {}

export interface MigrationExecutingStatus
  extends MigrationStatusBase<'MigrationExecutingStatus'> {}

export interface MigrationRollbackStatus
  extends MigrationStatusBase<'MigrationRollbackStatus'> {}

export interface MigrationRollbackSuccessStatus
  extends MigrationStatusBase<'MigrationRollbackSuccessStatus'> {}

export interface MigrationAppliedStatus
  extends MigrationStatusBase<'MigrationAppliedStatus'> {}

export interface MigrationAppliedAndSavedStatus
  extends MigrationStatusBase<'MigrationAppliedAndSavedStatus'> {}

export interface MigrationErrorStatus
  extends MigrationStatusBase<'MigrationErrorStatus'> {
  exception: unknown;
}

export interface MigrationRollbackErrorStatus
  extends MigrationStatusBase<'MigrationRollbackErrorStatus'> {
  exception: unknown;
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
