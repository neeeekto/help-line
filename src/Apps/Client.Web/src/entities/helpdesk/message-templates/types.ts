import { LanguageDictionary } from '@entities/common';

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
  id: string;
  order: number;
  modifyDate: string; // DateTime
}
