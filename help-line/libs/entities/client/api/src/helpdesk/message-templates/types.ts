import { GUID, LanguageDictionary } from '@help-line/entities/share';

export interface MessageTemplateContent {
  title: string;
  message: {
    text: string;
    attachments: [string];
  };
}

export interface MessageTemplateData {
  group: string;
  content: LanguageDictionary<MessageTemplateContent>;
}

export interface MessageTemplate extends MessageTemplateData {
  id: GUID;
  order: number;
  modifyDate: string; // DateTime
}
