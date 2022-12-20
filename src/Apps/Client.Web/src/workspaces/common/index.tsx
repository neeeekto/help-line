import React from "react";
import { useRouteMatch, generatePath } from "react-router";
import {
  Switch,
  Route,
  Redirect,
  useLocation,
  HashRouter,
} from "react-router-dom";
import { Spin } from "antd";

import { LayoutRoot } from "./layout";
import { usePathMaker } from "@core/router";
import { ProjectGuard } from "@core/system/components";
import { Project, useProjectsQueries } from "@entities/helpdesk/projects";
import { HdRoutes, UARoutes } from "./routes";

const RedirectToFirstProject: React.FC<{ projects?: Project[] }> = ({
  projects,
}) => {
  const make = usePathMaker();
  const { path } = useRouteMatch();
  if (!projects?.length) {
    return <div>No projects</div>;
  }
  const project = projects[0];
  const newPath = path.replace(":game", project.id);

  return (
    <Redirect to={newPath.includes(project.id) ? newPath : make(project.id)} />
  );
};

const RedirectToNewProject: React.FC<{ projectId: Project["id"] }> = ({
  projectId,
}) => {
  const location = useLocation();
  const math = useRouteMatch();
  const newPath = location.pathname.replace(
    math.url,
    generatePath(math.path, { projectId })
  );
  return <Redirect to={newPath} />;
};

const ProjectRoute: React.FC<{ prefix: string }> = ({ prefix, children }) => {
  const make = usePathMaker();
  return <Route path={make(prefix)}>{children}</Route>;
};

export const CommonWorkspace: React.FC = () => {
  const make = usePathMaker();
  const projectsQuery = useProjectsQueries();
  return (
    <LayoutRoot>
      {projectsQuery.isLoading ? (
        <Spin />
      ) : (
        <>
          <Switch>
            <Route path={make("ua")}>
              <UARoutes />
            </Route>
            <Route path={make(":projectId")}>
              <ProjectGuard
                projectKey="projectId"
                projects={projectsQuery.data!}
                unknownProject={
                  <RedirectToFirstProject projects={projectsQuery.data!} />
                }
                otherProject={(projectId) => (
                  <RedirectToNewProject projectId={projectId} />
                )}
              >
                <ProjectRoute prefix="hd">
                  <HdRoutes />
                </ProjectRoute>
              </ProjectGuard>
            </Route>

            <Route path={make()} exact>
              <RedirectToFirstProject projects={projectsQuery.data!} />
            </Route>
          </Switch>
          <HashRouter>
            <Route path="/create-ticket">Test</Route>
          </HashRouter>
        </>
      )}
    </LayoutRoot>
  );
};
