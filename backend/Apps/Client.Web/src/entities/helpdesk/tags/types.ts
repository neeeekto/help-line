import { LanguageDictionary } from "@entities/common";

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

export interface TagDescription {
  issues: TagDescriptionIssue[];
  enabled: boolean;
}

export interface TagDescriptionItem extends TagDescription {
  tag: string;
}
