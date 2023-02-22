import { PropsWithChildren, useMemo } from 'react';
import { EditStoreContext } from './context';
import {
  ComponentAdminApi,
  ContextAdminApi,
  TemplateAdminApi,
} from '@help-line/entities/admin/api';
import { makeEditorStore } from './store';
import { useInjection } from 'inversify-react';

export const EditorStoreProvider = ({ children }: PropsWithChildren) => {
  const templateApi = useInjection(TemplateAdminApi);
  const contextApi = useInjection(ContextAdminApi);
  const componentApi = useInjection(ComponentAdminApi);

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
