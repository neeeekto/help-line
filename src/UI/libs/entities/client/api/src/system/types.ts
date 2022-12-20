export interface AppState {
  blocked: boolean;
}

export enum SystemMessageLvl {
  Info = 'Info',
  Warning = 'Warning',
  Danger = 'Danger',
}

export interface SystemMessageData {
  text: string;
  lvl: SystemMessageLvl;
  showAt?: string;
  hideAt?: string;
  willHappenAt?: string;
}

export interface SystemMessage {
  id: string; // guid
  createDate: string; // DateTime
  data: SystemMessageData;
}

export interface Settings {
  languages: string[];
  defaultLanguage: string;
}
