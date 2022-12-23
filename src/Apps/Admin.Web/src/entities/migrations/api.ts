import { httpClient, HttpClient } from "@core/http";
import { Migration, MigrationStatus } from "./types";
import { WithType } from "@entities/common";
import { Description } from "@entities/meta.types";

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export const makeMigrationApi = (http: HttpClient) => ({
  get: () => http.get<Migration[]>("/api/v1/migrations").then((x) => x.data),

  run: (migration: string, params?: WithType<string>) =>
    http
      .post<Migration[]>(
        `/api/v1/migrations/${migration}`,
        { params: params || null },
        {
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
          },
        }
      )
      .then((x) => x.data),
  getParamsDescriptions: () =>
    http
      .get<Record<string, Description>>(
        `/api/v1/migrations/descriptions/params`
      )
      .then((x) => x.data),
});

export const migrationApi = makeMigrationApi(httpClient);
