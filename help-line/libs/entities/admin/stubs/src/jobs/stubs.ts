import { createStubFactory } from '@help-line/dev/http-stubs';
import { Job, JobData, JobTriggerState } from '@help-line/entities/admin/api';
import { faker } from '@faker-js/faker';

export namespace AdminJobsStubs {
  export const createJobData = createStubFactory<JobData>(() => ({
    name: faker.random.word(),
    schedule: '* * * ? * *',
  }));
  export const createJob = createStubFactory<Job>(() => ({
    ...createJobData(),
    id: faker.datatype.uuid(),
    taskType: faker.datatype.string(),
    enabled: true,
    modificationDate: faker.datatype.datetime().toISOString(),
  }));

  export const createJobTriggerState = createStubFactory<JobTriggerState>(
    () => ({
      prev: faker.datatype.datetime({ max: Date.now() }).toISOString(),
      next: faker.datatype.datetime({ min: Date.now() }).toISOString(),
    })
  );
}
