export interface TagCondition {
  include: boolean;
  tags: string[];
  all: boolean;
}

export interface AutoReplyData {
  name: string;
  enabled: boolean;
  projectId: string;
  weight: number;
  tagConditions: TagCondition[];
  operatorId: string;
  permanently: boolean;
}

export interface AutoReply extends AutoReplyData {
  id: string;
}
