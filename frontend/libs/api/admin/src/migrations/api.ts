import { ApiBase } from '@help-line/http';
import { Migration, MigrationStatus } from './types';
import { WithType, Description } from '@help-line/api/share';

export class MigrationAdminApi extends ApiBase {
  public async get() {
    return this.http.get<Migration[]>('/v1/migrations/').then((x) => x.data);
  }

  public async run(migration: string, params?: WithType<string>) {
    return this.http
      .post<Migration[]>(
        `/v1/migrations/${migration}/`,
        { params: params || null },
        {
          headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
          },
        }
      )
      .then((x) => x.data);
  }
  public async getParamsDescriptions() {
    return this.http
      .get<Record<string, Description>>(`/v1/migrations/descriptions/params/`)
      .then((x) => x.data);
  }
}
