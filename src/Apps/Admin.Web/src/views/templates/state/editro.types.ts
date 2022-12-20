import { TemplateItem } from "@entities/templates";

export enum SourceType {
  Template = "template",
  Context = "context",
  Component = "component",
}

export interface EditedItem<T extends TemplateItem = TemplateItem> {
  src: SourceType;
  id: string;
  original?: T;
  current: T;
}

export interface Opened<T extends TemplateItem = TemplateItem> {
  id: string;
  lang: string;
  src: SourceType;
  field: string;
  active: boolean;
  value?: string; // Optional value
}
