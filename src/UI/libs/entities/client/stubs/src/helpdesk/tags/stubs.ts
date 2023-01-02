import { createStubFactory } from '@help-line/dev/http-stubs';
import { faker } from '@faker-js/faker';
import {
  Tag,
  TagDescription,
  TagDescriptionIssue,
} from '@help-line/entities/client/api';

export namespace HelpdeskTagsClientStubs {
  export const createTag = createStubFactory<Tag>(() => ({
    key: faker.random.word(),
    enabled: faker.datatype.boolean(),
  }));

  export const createTagDescIssue = createStubFactory<TagDescriptionIssue>(
    () => ({
      audience: faker.datatype.array().map(() => faker.name.jobTitle()),
      contents: {
        en: {
          text: faker.lorem.text(),
          uri: faker.internet.url(),
        },
      },
    })
  );

  export const createTagDesc = createStubFactory<TagDescription>(() => ({
    tag: faker.random.word(),
    issues: faker.datatype.array().map(() => createTagDescIssue()),
    enabled: faker.datatype.boolean(),
  }));
}
