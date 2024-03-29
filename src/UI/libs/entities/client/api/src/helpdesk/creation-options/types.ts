import { LanguageDictionary } from '@help-line/entities/share';

export interface Platform {
  key: string;
  name: string;
  icon?: string;
  sort?: number;
}

export interface ProblemAndThemeContent {
  title: string;
  notification: string;
}

export interface ProblemAndTheme {
  tag: string;
  enabled: boolean;
  platformsTag: string[];
  content: LanguageDictionary<ProblemAndThemeContent>;
  subtypes?: ProblemAndTheme[];
}
