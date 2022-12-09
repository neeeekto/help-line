import { createStubFactory } from '@help-line/modules/http-stubs';
import {
  Component,
  Context,
  Project,
  Template,
  TemplateBase,
} from '@help-line/entities/admin/api';
import { faker } from '@faker-js/faker';

export namespace AdminTemplateRendererStubs {
  export const createTemplateItemData = createStubFactory<TemplateBase>(() => ({
    id: faker.name.firstName(),
    group: void 0,
    updatedAt: faker.datatype.datetime().toISOString(),
  }));

  export const createComponent = createStubFactory<Component>(() => ({
    ...createTemplateItemData(),
    content: '',
  }));

  export const createContext = createStubFactory<Context>(() => ({
    ...createTemplateItemData(),
    data: {},
  }));

  export const createTemplate = createStubFactory<Template>(() => ({
    ...createComponent(),
    contexts: [],
    name: faker.name.fullName(),
    props: {},
  }));
}
