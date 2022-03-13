import { Project } from "@entities/helpdesk/projects";

export const EN_LANGUAGE = "en";

export const makeProjectTestData = (data: Partial<Project> = {}): Project => ({
  id: "test",
  active: true,
  info: {
    image: "",
    name: "test",
  },
  languages: [EN_LANGUAGE],
  ...data,
});
