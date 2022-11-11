/* eslint-disable @typescript-eslint/no-empty-interface */
import { WithType, Filter } from '@help-line/api/share';
import { Operator } from '../operators';

export enum TicketStatusType {
  New = 'New', // opened, pending
  Answered = 'Answered', // opened, pending
  AwaitingReply = 'AwaitingReply', // opened, pending
  Resolved = 'Resolved', // opened, closed
  Rejected = 'Rejected', // closed
  ForReject = 'ForReject', // opened
}

export enum TicketStatusKind {
  Opened = 'Opened',
  Closed = 'Closed',
  Pending = 'Pending',
}

export interface TicketStatus {
  kind: TicketStatusKind;
  type: TicketStatusType;
}

export enum TicketPriority {
  Low = 'Low',
  Normal = 'Normal',
  High = 'High',
}

export enum TicketDiscussionStateMessageType {
  Incoming = 'Incoming',
  Outgoin = 'Outgoin',
}

export interface TicketDiscussionState {
  lastReplyDate: string; // StringDate
  lastMessageType: TicketDiscussionStateMessageType;
  iterationCount: number;
}

export interface TicketFeedback {
  dateTime: string; // StringDate
  feedbackId: string;
  score: number;
  message: string;
  solved: boolean;
  optionalScores: Record<string, number>;
}

export enum UserIdType {
  Main = 'Main',
  Linked = 'Linked',
}

export interface UserIdInfo {
  userId: string;
  channel: string;
  type: UserIdType;
  useForDiscussion: boolean;
}

export interface OperatorInitiator extends WithType<'OperatorInitiatorView'> {
  operatorId: string;
}

export interface SystemInitiator extends WithType<'SystemInitiatorView'> {
  description?: string;
  meta?: Record<string, string>;
}

export interface UserInitiator extends WithType<'UserInitiatorView'> {
  userId: string;
}

export type TicketInitiator =
  | OperatorInitiator
  | SystemInitiator
  | UserInitiator;

export interface TicketEventBase {
  id: string;
  initiator: TicketInitiator;
  createDate: string;
}

export enum ApproveState {
  Waiting = 'Waiting',
  Approved = 'Approved',
  Canceled = 'Canceled',
  Denied = 'Denied',
}
export interface TicketApprovalStatusEvent
  extends TicketEventBase,
    WithType<'TicketApprovalStatusEventView'> {
  rejectId: string;
  message: string;
  forStatus: TicketStatus;
  state: ApproveState;
}

export interface TicketAssigmentBindingEvent
  extends TicketEventBase,
    WithType<'TicketAssigmentBindingEventView'> {
  hardAssigment: boolean;
}

export interface TicketAssigmentEvent
  extends TicketEventBase,
    WithType<'TicketAssigmentEventView'> {
  from: string;
  to: string;
}

export interface TicketMeta {
  fromTicket?: string;
  source: string;
  platform?: string;
}

export interface Message {
  text: string;
  attachments?: string[];
}

export interface TicketCreatedEvent
  extends TicketEventBase,
    WithType<'TicketCreatedEventView'> {
  projectId: string;
  tags: string[];
  language: string;
  userIds: UserIdInfo[];
  userMeta: Record<string, string>;
  meta: TicketMeta;
  status: TicketStatus;
  priority: TicketPriority;
  message?: Message;
}
export interface TicketFeedbackEvent
  extends TicketEventBase,
    WithType<'TicketFeedbackEventView'> {
  feedbackId: string;
}

export interface TicketIncomingMessageEvent
  extends TicketEventBase,
    WithType<'TicketIncomingMessageEventView'> {
  message: Message;
  channel: string;
}

export interface TicketLanguagesChangedEvent
  extends TicketEventBase,
    WithType<'TicketLanguagesChangedEventView'> {
  from: string;
  to: string;
}

export enum TicketLifeCycleType {
  Resolving = 'Resolving',
  Feedback = 'Feedback',
  Closing = 'Closing',
}

export interface ScheduleResultBase {
  date: string; // DateTime
  initiator: TicketInitiator;
}

export type ScheduleCanceledResult = ScheduleResultBase &
  WithType<'ScheduledEventCanceledResultView'>;
export type ScheduleDoneResult = ScheduleResultBase &
  WithType<'ScheduledEventDoneResultView'>;
export type ScheduleResult = ScheduleDoneResult | ScheduleCanceledResult;

export interface TicketLifecycleEvent
  extends TicketEventBase,
    WithType<'TicketLifecycleEventView'> {
  type: TicketLifeCycleType;
  result: ScheduleResult;
  executionDate: string; // DateTime
}

export interface TicketNoteHistoryRecord {
  initiator: TicketInitiator;
  message: Message;
  date: string; // DateTime
}

export interface TicketNoteEvent
  extends TicketEventBase,
    WithType<'TicketNoteEventView'> {
  noteId: string;
  message: Message;
  tags: string[];
  deleteTime?: string; //DateTime
  history: TicketNoteHistoryRecord[];
}

export enum MessageStatus {
  Sending = 'Sending',
  Sent = 'Sent',
  Delivered = 'Delivered',
  Viewed = 'Viewed',
  NotDelivered = 'NotDelivered',
}
export interface DeliveryStatus {
  date: string; // DateTime
  status: MessageStatus;
  detail?: string;
}

export interface Recipient {
  userId: string;
  channel: string;
  deliveryStatuses: DeliveryStatus[];
}

export interface TicketOutgoingMessageEvent
  extends TicketEventBase,
    WithType<'TicketOutgoingMessageEventView'> {
  messageId: string;
  message: Message;
  recipients: Recipient[];
}

export interface TicketPriorityEvent
  extends TicketEventBase,
    WithType<'TicketPriorityEventView'> {
  old: TicketPriority;
  new: TicketPriority;
}

export interface ReminderView {
  id: string;
  message: Message;
  sendDate: string; // DateTime
  resolving: boolean;
  next?: ReminderView;
}

export interface TicketReminderEvent
  extends TicketEventBase,
    WithType<'TicketReminderEventView'> {
  reminder: ReminderView;
  result: ScheduleResult;
}

export interface TicketStatusChangedEvent
  extends TicketEventBase,
    WithType<'TicketStatusChangedEventView'> {
  old: TicketStatus;
  new: TicketStatus;
}

export interface TicketTagsChangedEvent
  extends TicketEventBase,
    WithType<'TicketTagsChangedEventView'> {
  old: string[];
  new: string[];
}

export interface TicketUserIdsChangedEvent
  extends TicketEventBase,
    WithType<'TicketUserIdsChangedEventView'> {
  old: UserIdInfo[];
  new: UserIdInfo[];
}

export interface TicketUserMetaChangedEvent
  extends TicketEventBase,
    WithType<'TicketUserMetaChangedEventView'> {
  old: Record<string, string>;
  new: Record<string, string>;
}

export interface TicketUserUnsubscribedEvent
  extends TicketEventBase,
    WithType<'TicketUserUnsubscribedEventView'> {
  userId: string;
  message: string;
}

export type TicketEvent =
  | TicketUserUnsubscribedEvent
  | TicketUserMetaChangedEvent
  | TicketUserIdsChangedEvent
  | TicketTagsChangedEvent
  | TicketStatusChangedEvent
  | TicketReminderEvent
  | TicketPriorityEvent
  | TicketOutgoingMessageEvent
  | TicketLifecycleEvent
  | TicketLanguagesChangedEvent
  | TicketIncomingMessageEvent
  | TicketFeedbackEvent
  | TicketApprovalStatusEvent
  | TicketAssigmentBindingEvent
  | TicketAssigmentEvent
  | TicketCreatedEvent;

export interface Ticket {
  id: string;
  projectId: string;
  tags: string[];
  language: string;
  status: TicketStatus;
  priority: TicketPriority;
  hardAssigment: boolean;
  assignedTo?: string; // OperatorID
  createDate: string; // StringDate
  dateOfLastStatusChange: string; // StringDate
  discussionState: TicketDiscussionState;
  title: string;
  feedbacks: TicketFeedback[];
  userIds: UserIdInfo[];
  userMeta: Record<string, string>;
  events: TicketEvent[];
}

export interface TicketReminderDataBase {
  delay: string; // TimeSpan
  message: Message; // TimeSpan
}

export interface TicketSequentialReminderData
  extends TicketReminderDataBase,
    WithType<'TicketSequentialReminderDto'> {
  next: TicketReminderData;
}

export interface TicketFinalReminderData
  extends TicketReminderDataBase,
    WithType<'TicketFinalReminderDto'> {
  resolve: boolean;
}

export type TicketReminderData =
  | TicketSequentialReminderData
  | TicketFinalReminderData;

export interface UserIdData {
  userId: string;
  channel: string;
  useForDiscussion: boolean;
  main: boolean;
}

export interface CreateTicketData {
  language: string;
  tags: string[];
  channels: Record<string, string>; // {userId: channelKey}
  userMeta: Record<string, string>;
  message?: Message;
  fromTicket?: string;
}

export interface TicketsSettings {
  projectId: string;
  lifeCycleDelay: Record<TicketLifeCycleType, string>;
  inactivityDelay: string;
  feedbackCompleteDelay: string;
}

export enum TicketFilterFeatures {
  Important = 'Important',
  Automations = 'Automations',
}

export interface TicketFilterShareGlobal
  extends WithType<'TicketFilterShareGlobal'> {}

export interface TicketFilterShareForOperators
  extends WithType<'TicketFilterShareForOperators'> {
  operators: Array<Operator['id']>;
}

export interface TicketFilterData {
  name: string;
  filter: Filter;
  share?: TicketFilterShareGlobal | TicketFilterShareForOperators;
  features: TicketFilterFeatures[]; // for client features, eg: global, default, counters, important - server dont know about it
  order: number; // View order in filters list
}

export interface TicketFilter extends TicketFilterData {
  id: string;
  changed: string; // DateTime
  owner?: Operator['id']; // null is system, GUid - operator ID
}

// Actions
export interface AddOutgoingMessageTicketAction
  extends WithType<'AddOutgoingMessageAction'> {
  message: Message;
}

export interface AddTicketNoteAction extends WithType<'AddTicketNoteAction'> {
  message: Message;
  tags: string[];
}

export interface AddTicketReminderAction
  extends WithType<'AddTicketReminderAction'> {
  reminder: TicketReminderData;
}

export interface AddUserIdAction extends WithType<'AddUserIdAction'> {
  userId: string;
  channel: string;
  useForDiscussion: boolean;
  main: boolean;
}

export interface ApproveTicketRejectAction
  extends WithType<'ApproveTicketRejectAction'> {
  auditId: string;
}

export interface AssignAction extends WithType<'AssignAction'> {
  operatorId: string;
}

export interface CancelTicketRejectAction
  extends WithType<'CancelTicketRejectAction'> {
  rejectId: string;
}

export interface CancelTicketReminderAction
  extends WithType<'CancelTicketReminderAction'> {
  reminderId: string;
}

export interface ChangeLanguageAction extends WithType<'ChangeLanguageAction'> {
  language: string;
}

export interface ChangePriorityAction extends WithType<'ChangePriorityAction'> {
  priority: TicketPriority;
}

export interface ChangeTagsAction extends WithType<'ChangeTagsAction'> {
  tags: string[];
}

export interface ChangeTicketNoteAction
  extends WithType<'ChangeTicketNoteAction'> {
  noteId: string;
  message: Message;
  tags: string[];
}

export interface ChangeUserMetaAction extends WithType<'ChangeUserMetaAction'> {
  meta: Record<string, string>;
}

export interface DenyTicketRejectAction
  extends WithType<'DenyTicketRejectAction'> {
  rejectId: string;
  message: string;
}

export interface ImmediateSendFeedbackAction
  extends WithType<'ImmediateSendFeedbackAction'> {}

export interface RejectTicketAction extends WithType<'RejectTicketAction'> {
  message: string;
}

export interface RemoveTicketNoteAction
  extends WithType<'RemoveTicketNoteAction'> {
  noteId: string;
}

export interface RemoveUserIdAction extends WithType<'RemoveUserIdAction'> {
  userId: string;
}

export interface ReopenTicketAction extends WithType<'ReopenTicketAction'> {}

export interface ResolveTicketAction extends WithType<'ResolveTicketAction'> {}

export interface ToggleHardAssigmentAction
  extends WithType<'ToggleHardAssigmentAction'> {
  hardAssigment: boolean;
}

export interface TogglePendingAction extends WithType<'TogglePendingAction'> {
  pending: boolean;
}

export interface ToggleUserChannelAction
  extends WithType<'ToggleUserChannelAction'> {
  userId: string;
  enabled: boolean;
}

export interface UnassignAction extends WithType<'UnassignAction'> {}

export type TicketAction =
  | AddOutgoingMessageTicketAction
  | AddTicketNoteAction
  | AddTicketReminderAction
  | AddUserIdAction
  | ApproveTicketRejectAction
  | AssignAction
  | CancelTicketRejectAction
  | CancelTicketReminderAction
  | ChangeLanguageAction
  | ChangePriorityAction
  | ChangeTagsAction
  | ChangeTicketNoteAction
  | ChangeUserMetaAction
  | DenyTicketRejectAction
  | ImmediateSendFeedbackAction
  | RejectTicketAction
  | RemoveTicketNoteAction
  | RemoveUserIdAction
  | ReopenTicketAction
  | ResolveTicketAction
  | ToggleHardAssigmentAction
  | TogglePendingAction
  | ToggleUserChannelAction
  | UnassignAction;
