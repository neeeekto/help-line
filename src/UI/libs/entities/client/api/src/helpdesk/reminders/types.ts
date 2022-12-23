import { LanguageDictionary, WithType } from '@entities/common';
import { Message, TicketReminderData } from '@entities/helpdesk/tickets';

export interface SavedReminderItemBase {
  delay: string; // TimeSpan
  message: LanguageDictionary<Message>;
}

export interface SequentialSavedReminderItem extends SavedReminderItemBase, WithType<'TicketReminderItemBase'> {
  next: TicketReminderData;
}

export interface FinalSavedReminderItem extends SavedReminderItemBase, WithType<'TicketReminderItemBase'> {
  resolve: boolean;
}

export type ReminderItem = SequentialSavedReminderItem | FinalSavedReminderItem;

export interface SavedReminderData {
  enabled: true;
  group: string;
  name: string;
  description: string;
  reminders: ReminderItem;
}
export interface SavedReminder extends SavedReminderData {
  id: string;
}
