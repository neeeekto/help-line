import { createStubFactory } from '@help-line/dev/http-stubs';
import { faker } from '@faker-js/faker';
import { Project } from '@help-line/entities/client/api';

export namespace HelpdeskProjectsClientStubs {
  export const createProject = createStubFactory<Project>(() => ({
    id: faker.datatype.uuid(),
    languages: Array(faker.datatype.number({ min: 1, max: 10 }))
      .fill(null)
      .map(faker.random.locale),
    info: {
      name: faker.name.jobTitle(),
      image: faker.image.avatar(),
    },
    active: true,
  }));
}
