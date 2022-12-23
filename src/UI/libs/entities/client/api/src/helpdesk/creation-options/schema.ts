import { makeRudSchema } from '../../api.presets';
import {
  makeHeaderWithProject,
  Platform,
  ProblemAndTheme,
  ProjectApiRequest,
} from '@help-line/entities/client/api';

export const CreationOptionsPlatformClientApiSchema = makeRudSchema<
  Platform,
  Platform,
  Platform['key'],
  ProjectApiRequest
>('/v1/hd/creation-options/platforms', makeHeaderWithProject);

export const CreationOptionsProblemAndThemesClientApiSchema = makeRudSchema<
  ProblemAndTheme,
  ProblemAndTheme,
  ProblemAndTheme['tag'],
  ProjectApiRequest
>('/v1/hd/creation-options/problems-and-themes', makeHeaderWithProject);
