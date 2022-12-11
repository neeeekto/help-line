import { LanguageDictionary } from '@help-line/entities/share';

export interface Tag {
  key: string;
  enabled: boolean;
}

export interface TagDescriptionContent {
  text: string;
  uri?: string;
}

export interface TagDescriptionIssue {
  contents: LanguageDictionary<TagDescriptionContent>;
  audience: string[];
}

export interface TagDescriptionData {
  issues: TagDescriptionIssue[];
  enabled: boolean;
}

export interface TagDescription extends TagDescriptionData {
  tag: string;
}
