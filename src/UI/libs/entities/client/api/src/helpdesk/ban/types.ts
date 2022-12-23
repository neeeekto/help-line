import { GUID } from '@help-line/entities/share';

export enum BanType {
  Ip = 'Ip',
  Text = 'Text',
}

export interface CreateBanData {
  parameter: BanType;
  value: string;
  expiredAt: string; // Date
}

export interface Ban extends CreateBanData {
  readonly id: GUID;
  readonly projectId: string;
}
