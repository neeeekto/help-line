import { createStubFactory } from '@help-line/dev/http-stubs';
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
    content: faker.lorem.text(),
  }));

  export const createContext = createStubFactory<Context>(() => ({
    ...createTemplateItemData(),
    data: JSON.parse(faker.datatype.json()),
  }));

  export const createTemplate = createStubFactory<Template>(() => ({
    ...createComponent(),
    contexts: [],
    name: faker.name.fullName(),
    props: JSON.parse(faker.datatype.json()),
  }));
}
