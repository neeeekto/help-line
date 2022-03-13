import { httpClient, makeRudApi } from "@core/http";
import { Platform, ProblemAndTheme } from "./types";

export const creationOptionsPlatformApi = makeRudApi<
  Platform,
  Platform["name"],
  string
>(httpClient, "/api/v1/hd/creation-options/platforms");

export const creationOptionsProblemAndThemesApi = makeRudApi<
  ProblemAndTheme,
  ProblemAndTheme,
  string
>(httpClient, "/api/v1/hd/creation-options/problems-and-themes");
