import { Project } from "@entities/helpdesk/projects";

export const EN_LANGUAGE = "en";
export const PROJECT_TEST_ID = "test";

export const makeProjectTestData = (data: Partial<Project> = {}): Project => ({
  id: PROJECT_TEST_ID,
  active: true,
  info: {
    image: "",
    name: "test",
  },
  languages: [EN_LANGUAGE],
  ...data,
});
