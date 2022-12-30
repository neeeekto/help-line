import { createStubFactory } from '@help-line/dev/http-stubs';
import { Project } from '@help-line/entities/admin/api';
import { faker } from '@faker-js/faker';

export namespace AdminProjectStubs {
  export const createProject = createStubFactory<Project>(() => ({
    activeTab: true,
    id: faker.datatype.uuid(),
    languages: Array(faker.datatype.number({ min: 1, max: 10 }))
      .fill(null)
      .map(faker.random.locale),
    info: {
      name: faker.name.jobTitle(),
      image: faker.image.avatar(),
    },
  }));
}
