import { Project } from "@entities/helpdesk/projects";
import { lcManager } from "@shared/utils/local-storage";
import { observable, action } from "mobx";

const LC_PROJECT = "project";

export const makeSystemStore = () => {
  const state = observable({
    currentProject: lcManager.get<string>(LC_PROJECT),
  });
  const setProject = action("setProject", (project: Project) => {
    lcManager.set(LC_PROJECT, project.id);
    state.currentProject = project.id;
  });

  return {
    state,
    setProject,
  };
};

export type SystemStore = ReturnType<typeof makeSystemStore>;
