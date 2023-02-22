import { PropsWithChildren, useEffect, useMemo } from 'react';
import { Provider } from 'inversify-react';
import { Container } from 'inversify';
import {
  ComponentAdminApi,
  ContextAdminApi,
  HelpdeskAdminApi,
  JobsAdminApi,
  MigrationAdminApi,
  ProjectAdminApi,
  TemplateAdminApi,
} from '@help-line/entities/admin/api';
import { AxiosHttpBackend, HttpClient } from '@help-line/modules/http';
import {
  AuthInterceptor,
  useAuthHttpAccessorRef,
} from '@help-line/modules/application';

export const DiProvider = (props: PropsWithChildren & { apiUrl?: string }) => {
  const authHttpAccessor = useAuthHttpAccessorRef();

  const container = useMemo(() => {
    const diContainer = new Container({ defaultScope: 'Singleton' });
    const httpClient = new HttpClient(
      new AxiosHttpBackend(props.apiUrl || ''),
      [new AuthInterceptor(authHttpAccessor)]
    );
    diContainer.bind(HttpClient).toConstantValue(httpClient);
    diContainer
      .bind(MigrationAdminApi)
      .toConstantValue(new MigrationAdminApi(httpClient));

    diContainer
      .bind(HelpdeskAdminApi)
      .toConstantValue(new HelpdeskAdminApi(httpClient));

    diContainer
      .bind(JobsAdminApi)
      .toConstantValue(new JobsAdminApi(httpClient));

    diContainer
      .bind(ProjectAdminApi)
      .toConstantValue(new ProjectAdminApi(httpClient));

    diContainer
      .bind(TemplateAdminApi)
      .toConstantValue(new TemplateAdminApi(httpClient));
    diContainer
      .bind(ContextAdminApi)
      .toConstantValue(new ContextAdminApi(httpClient));
    diContainer
      .bind(ComponentAdminApi)
      .toConstantValue(new ComponentAdminApi(httpClient));
    return diContainer;
  }, [authHttpAccessor]);

  return <Provider container={container}>{props.children}</Provider>;
};
