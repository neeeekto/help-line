import { createQueryKeys } from '@help-line/modules/query';
import { useMutation, useQuery } from '@tanstack/react-query';
import { EditCache, Resource } from './index';
import { useEditStore } from './index';

export const uiAdminTemplatesQueryKeys = createQueryKeys(
  ['ui', 'admin', 'templates'],
  ({ makeKey }) => ({
    init: makeKey('init'),
  })
);

export const useInitQuery = () => {
  const store$ = useEditStore();
  return useQuery(
    uiAdminTemplatesQueryKeys.init,
    () => store$.init().then((x) => ''),
    {
      staleTime: Number.POSITIVE_INFINITY,
    }
  );
};

export const useDeleterResourceMutation = (resourceId: Resource['id']) => {
  const store$ = useEditStore();
  return useMutation(
    [...uiAdminTemplatesQueryKeys.root, 'delete', resourceId],
    () => store$.actions.deleteResource(resourceId)
  );
};

export const useSaveResourceMutation = (resourceId: Resource['id']) => {
  const store$ = useEditStore();
  return useMutation(
    [...uiAdminTemplatesQueryKeys.root, 'save', resourceId],
    () => store$.actions.saveResources(resourceId)
  );
};
