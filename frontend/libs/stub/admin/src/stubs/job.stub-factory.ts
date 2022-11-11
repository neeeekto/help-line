import { Job, JobData } from '@help-line/api/admin';
import { createStubFactory } from '@help-line/stub/share';
import { faker } from '@faker-js/faker';

export const jobStubFactory = {
  createJobData: createStubFactory<JobData>(() => ({
    name: faker.name.firstName(),
    group: faker.datatype.uuid(),
    schedule: '* * * ? * *',
  })),

  createJob: createStubFactory<Job>(() => ({
    name: faker.name.firstName(),
    group: faker.datatype.uuid(),
    schedule: '* * * ? * *',
    id: faker.datatype.uuid(),
    taskType: faker.datatype.string(),
    enabled: true,
    modificationDate: faker.datatype.datetime().toDateString(),
  })),
};
