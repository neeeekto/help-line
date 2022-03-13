export enum BanType {
  Ip = 'Ip',
  Text = 'Text',
}

export interface Ban {
  readonly id: string;
  projectId: string;
  parameter: BanType;
  value: string;
  expiredAt: string; // Date
}

export interface BanSettings {
  projectId: string;
  banDelay: string;
  ticketsCount: number;
  interval: string;
}
