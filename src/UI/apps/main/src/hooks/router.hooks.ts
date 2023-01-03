import { useParams } from 'react-router-dom';
import { Project } from '@help-line/entities/client/api';

export const useProjectIdParam = () => {
  const { projectId } = useParams<{ projectId: Project['id'] }>();
  return projectId;
};
