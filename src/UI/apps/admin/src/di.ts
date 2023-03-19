import {
  ComponentAdminApi,
  ContextAdminApi,
  HelpdeskAdminApi,
  JobsAdminApi,
  MigrationAdminApi,
  ProjectAdminApi,
  TemplateAdminApi,
} from '@help-line/entities/admin/api';
import { Newable } from 'ts-essentials';
import { DiRegistratorFn, setupDI } from '@help-line/modules/di';
import { HttpClient } from '@help-line/modules/http';
import { registryAuthInDI } from '@help-line/modules/auth';
import { IEnvironment, registryHttpInDi } from '@help-line/modules/application';

export const registryApiServicesInDI = (): DiRegistratorFn => (container) => {
  const apiCCtors = [
    MigrationAdminApi,
    HelpdeskAdminApi,
    JobsAdminApi,
    ProjectAdminApi,
    TemplateAdminApi,
    ContextAdminApi,
    ComponentAdminApi,
  ];
  apiCCtors.map((cctor: Newable<any>) =>
    container
      .bind(cctor)
      .toConstantValue(new cctor(container.resolve(HttpClient)))
  );
};

export const setupAppDI = (env: IEnvironment) =>
  setupDI(
    registryAuthInDI(env.oauth),
    registryHttpInDi(env.apiUrl),
    registryApiServicesInDI()
  );
