export interface AppState {
  blocked: boolean;
}

export enum MessageLvl {
  Info = "Info",
  Warning = "Warning",
  Danger = "Danger",
}

export interface MessageData {
  text: string;
  lvl: MessageLvl;
  showAt?: string;
  hideAt?: string;
  willHappenAt?: string;
}

export interface Message {
  id: string; // guid
  createDate: string; // DateTime
  data: MessageData;
}

export interface Settings {
  languages: string[];
  defaultLanguage: string;
}
