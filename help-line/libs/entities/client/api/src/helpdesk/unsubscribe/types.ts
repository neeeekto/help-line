import { GUID } from '@help-line/entities/share';

export interface Unsubscribe {
  id: GUID;
  userId: string;
  message: string;
  date: string; //DateTime
  projectId: string;
}
