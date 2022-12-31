import { PropsWithChildren, useEffect, useMemo } from 'react';
import { EditStoreContext } from './context';
import { useApi } from '@help-line/modules/api';
import {
  ComponentAdminApi,
  ContextAdminApi,
  TemplateAdminApi,
} from '@help-line/entities/admin/api';
import { makeEditorStore } from './store';

export const EditorStoreProvider = ({ children }: PropsWithChildren) => {
  const templateApi = useApi(TemplateAdminApi);
  const contextApi = useApi(ContextAdminApi);
  const componentApi = useApi(ComponentAdminApi);

  const store = useMemo(
    () => makeEditorStore(templateApi, contextApi, componentApi),
    [templateApi, contextApi, componentApi]
  );
  return (
    <EditStoreContext.Provider value={store}>
      {children}
    </EditStoreContext.Provider>
  );
};
