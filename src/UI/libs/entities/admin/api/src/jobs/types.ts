export interface JobData {
  name: string;
  group?: string;
  schedule: string; // CRON
  data?: any;
}

export interface Job extends JobData {
  readonly id: string;
  enabled: boolean;
  modificationDate: string; // DateTime
  readonly taskType: string;
}

export interface JobTriggerState {
  prev?: string; // DateTime;
  next?: string; //DateTime?
}
