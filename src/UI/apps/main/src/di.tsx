import { PropsWithChildren, useMemo } from 'react';
import { Provider, useContainer } from 'inversify-react';
import { Container } from 'inversify';
import { AxiosHttpBackend, HttpClient } from '@help-line/modules/http';
import {
  AuthInterceptor,
  useAuthHttpAccessorRef,
} from '@help-line/modules/application';
import {
  AutoRepliesClientApi,
  BanClientApi,
  CreationOptionsPlatformClientApi,
  CreationOptionsProblemAndThemesClientApi,
  MessageTemplateClientApi,
  OperatorsClientApi,
  OperatorsRolesClientApi,
  PreviewClientApi,
  ProjectsClientApi,
  RemindersClientApi,
  ReopenConditionsClientApi,
  RolesClientApi,
  SystemClientApi,
  TagDescriptionsClientApi,
  TagsClientApi,
  TicketClientApi,
  TicketsClientApi,
  TicketsFiltersClientApi,
  TicketsSettingClientApi,
  UnsubscribeClientApi,
  UsersClientApi,
} from '@help-line/entities/client/api';
import { Newable } from 'ts-essentials';
import { BanSettingsClientApi } from '../../../libs/entities/client/api/src/helpdesk/ban-settings';

export const DiProvider = (props: PropsWithChildren & { apiUrl?: string }) => {
  const authHttpAccessor = useAuthHttpAccessorRef();

  const container = useMemo(() => {
    const diContainer = new Container({ defaultScope: 'Singleton' });
    const httpClient = new HttpClient(
      new AxiosHttpBackend(props.apiUrl || ''),
      [new AuthInterceptor(authHttpAccessor)]
    );
    diContainer.bind(HttpClient).toConstantValue(httpClient);

    const apiCCtors = [
      UsersClientApi,
      RolesClientApi,
      SystemClientApi,
      AutoRepliesClientApi,
      BanClientApi,
      BanSettingsClientApi,
      CreationOptionsPlatformClientApi,
      CreationOptionsProblemAndThemesClientApi,
      MessageTemplateClientApi,
      OperatorsClientApi,
      OperatorsRolesClientApi,
      PreviewClientApi,
      ProjectsClientApi,
      RemindersClientApi,
      ReopenConditionsClientApi,
      TagsClientApi,
      TagDescriptionsClientApi,
      TicketsSettingClientApi,
      TicketsFiltersClientApi,
      TicketsClientApi,
      TicketClientApi,
      UnsubscribeClientApi,
    ];

    apiCCtors.map((cctor: Newable<any>) =>
      diContainer.bind(cctor).toConstantValue(new cctor(httpClient))
    );

    return diContainer;
  }, [authHttpAccessor]);

  return <Provider container={container}>{props.children}</Provider>;
};
