import React, { PropsWithChildren } from "react";
import { observer } from "mobx-react-lite";
import { useRouteMatch } from "react-router";
import { Project } from "@entities/helpdesk/projects";
import { useSystemStore$ } from "../system.context";

export const ProjectGuard = observer(
  (
    props: PropsWithChildren<{
      projectKey: string;
      projects: Project[];
      unknownProject: React.ReactNode;
      otherProject: (project: Project["id"]) => React.ReactNode;
    }>
  ) => {
    const systemStore = useSystemStore$();
    const routerMatch = useRouteMatch<any>();
    const project = routerMatch.params[props.projectKey];
    const projectByRouter = props.projects.find((x) => x.id === project);
    if (!systemStore.state.currentProject) {
      if (!projectByRouter) {
        return <>{props.unknownProject}</>;
      }
      systemStore.setProject(projectByRouter);
      return <>{props.children}</>;
    }
    if (project !== systemStore.state.currentProject) {
      return <>{props.otherProject(systemStore.state.currentProject!)}</>;
    }
    return <>{props.children}</>;
  }
);
