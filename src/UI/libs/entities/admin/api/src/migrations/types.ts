/* eslint-disable @typescript-eslint/no-empty-interface */
import { WithType } from '@help-line/entities/share';

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

export enum MigrationStatusType {
  InQueue = 'MigrationInQueueStatus',
  Executing = 'MigrationExecutingStatus',
  Rollback = 'MigrationRollbackStatus',
  RollbackSuccess = 'MigrationRollbackSuccessStatus',
  Applied = 'MigrationAppliedStatus',
  AppliedAndSaved = 'MigrationAppliedAndSavedStatus',
  Error = 'MigrationErrorStatus',
  RollbackError = 'MigrationRollbackErrorStatus',
}

export interface MigrationInQueueStatus
  extends MigrationStatusBase<MigrationStatusType.InQueue> {}

export interface MigrationExecutingStatus
  extends MigrationStatusBase<MigrationStatusType.Executing> {}

export interface MigrationRollbackStatus
  extends MigrationStatusBase<MigrationStatusType.Rollback> {}

export interface MigrationRollbackSuccessStatus
  extends MigrationStatusBase<MigrationStatusType.RollbackSuccess> {}

export interface MigrationAppliedStatus
  extends MigrationStatusBase<MigrationStatusType.Applied> {}

export interface MigrationAppliedAndSavedStatus
  extends MigrationStatusBase<MigrationStatusType.AppliedAndSaved> {}

export interface MigrationErrorStatus
  extends MigrationStatusBase<MigrationStatusType.Error> {
  exception: unknown;
}

export interface MigrationRollbackErrorStatus
  extends MigrationStatusBase<MigrationStatusType.RollbackError> {
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
