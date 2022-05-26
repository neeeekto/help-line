import { OperatorView } from "@entities/helpdesk/operators";
import { faker } from "@faker-js/faker";
import { PROJECT_TEST_ID } from "@test-utils/data/project.test-data";

export const makeOperatorViewTestData = (data: Partial<OperatorView> = {}) => {
  return {
    id: faker.datatype.uuid(),
    active: true,
    firstName: faker.name.firstName(),
    lastName: faker.name.lastName(),
    photo: faker.image.avatar(),
    projects: [PROJECT_TEST_ID],
    ...data,
  } as OperatorView;
};
