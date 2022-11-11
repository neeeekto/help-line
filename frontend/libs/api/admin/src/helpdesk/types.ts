import { KeyValue } from '@help-line/api/share';

export enum TicketScheduleStatus {
  Planned = 'Planned',
  InQueue = 'InQueue',
  Problem = 'Problem',
  Dead = 'Dead',
}

export interface TicketSchedule {
  id: string;
  ticketId: string;
  triggerDate: string; // DateTime
  createdAt: string; // DateTime
  status: TicketScheduleStatus;
  details?: string;
}

export interface ChannelItem {
  userId: string;
  channel: string;
}

export interface CreateTicketRequest {
  project: string;
  language: string;
  userId: string;
  tags: string[];
  channels: ChannelItem[];
  userMeta: Array<KeyValue<string, string>>; // {key: val} es: {device: phone, os: android 8.1}
  text: string;
  attachments?: string[];
  fromTicket?: string;
}
