export enum Channels {
  Email = "email",
  Chat = "chat",
}

export interface EmailRendererResult {
  body: string;
  subject: string;
}
