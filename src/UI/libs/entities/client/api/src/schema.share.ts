import { Project } from './helpdesk/projects';

export interface ProjectApiRequest {
  projectId: Project['id'];
}

export const makeHeaderWithProject = <T extends ProjectApiRequest>(req: T) => ({
  Project: req.projectId,
});
