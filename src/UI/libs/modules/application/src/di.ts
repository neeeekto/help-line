import { DiRegistratorFn } from '@help-line/modules/di';
import { AxiosHttpBackend, HttpClient } from '@help-line/modules/http';
import { AuthInterceptor } from './interceptors';
import { AuthStore } from '@help-line/modules/auth';

export const registryHttpInDi =
  (apiUrl: string): DiRegistratorFn =>
  (container) => {
    container
      .bind(HttpClient)
      .toConstantValue(
        new HttpClient(new AxiosHttpBackend(apiUrl), [
          new AuthInterceptor(container.resolve(AuthStore)),
        ])
      );
  };
