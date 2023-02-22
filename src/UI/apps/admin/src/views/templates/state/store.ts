import { observable, action, computed, values } from 'mobx';
import { computedFn } from 'mobx-utils';
import {
  EditTab,
  EditCache,
  ResourceType,
  Resource,
  ValueAccessor,
} from './types';
import {
  ComponentAdminApi,
  Context,
  ContextAdminApi,
  TemplateAdminApi,
  TemplateBase,
} from '@help-line/entities/admin/api';

export const makeEditorStore = (
  templateApi: TemplateAdminApi,
  contextApi: ContextAdminApi,
  componentApi: ComponentAdminApi
) => {
  const state = observable({
    resources: {} as Record<Resource['id'], Resource>,
    tabs: {} as Record<EditTab['id'], EditTab>,
    activeTab: '' as EditTab['id'],
    cache: {} as Record<Resource['id'], EditCache>,
  });

  const current = computed(() => {
    const tab = state.tabs[state.activeTab];
    if (tab) {
      const resource = state.resources[tab.resource];
      return { tab, resource };
    }
    return null;
  });

  const tabs = computed(() => {
    return values(state.tabs);
  });

  const resources = computed(() => {
    return values(state.resources);
  });

  const resourceByType = computedFn(
    <T extends TemplateBase = TemplateBase>(type: ResourceType) =>
      resources.get().filter((x) => x.type === type) as Array<Resource<T>>
  );

  const resourceByTab = computedFn(
    (tab: EditTab) => state.resources[tab.resource]
  );

  const isChanged = computedFn((resourceId: Resource['id']) => {
    return !!state.cache[resourceId];
  });

  const NEW_ITEMS_LS = 'admin_templates_new_items';
  const getResourcesFromLS = (): Resource[] => {
    try {
      return JSON.parse(localStorage.getItem(NEW_ITEMS_LS) || '') || [];
    } catch (e) {
      return [];
    }
  };
  const saveResourcesToLS = (items: Resource[]) => {
    localStorage.setItem(NEW_ITEMS_LS, JSON.stringify(items));
  };

  const init = action('editing.init', async () => {
    const [templates, contexts, components] = await Promise.all([
      templateApi.get(),
      contextApi.get(),
      componentApi.get(),
    ]);

    addResource(templates, ResourceType.Template);
    addResource(contexts, ResourceType.Context);
    addResource(components, ResourceType.Component);

    getResourcesFromLS().findIndex((x) => {
      state.resources[x.id] = x;
    });
  });

  const addResource = action(
    'editing.add',
    (data: TemplateBase[], type: ResourceType, isNew = false) => {
      for (let item of data) {
        const id = `${item.id}.${type}`;
        state.resources[id] = {
          id,
          data: item,
          type,
          hash: item.updatedAt,
          isNew,
        };
      }
      saveResourcesToLS(Object.values(state.resources).filter((x) => x.isNew));
    }
  );

  const updateResource = action(
    'editing.edit',
    (
      id: Resource['id'],
      hash: string,
      data: Partial<TemplateBase>,
      temp?: any
    ) => {
      state.cache[id] = {
        resource: id,
        hash,
        value: {
          ...state.cache[id]?.value,
          ...data,
        },
        temp,
      };
    }
  );

  const openTab = action('editing.open', (tab: EditTab) => {
    if (state.tabs[tab.id] !== tab) {
      state.tabs[tab.id] = tab;
    }
    state.activeTab = tab.id;
  });

  const closeTab = action('editing.close', (tabId: EditTab['id']) => {
    if (state.activeTab === tabId && tabs.get().length > 1) {
      const list = tabs.get();
      const inx = list.findIndex((x) => x.id === tabId);
      let nextTab = 0;
      if (inx + 1 < list.length || inx - 1 < 0) {
        nextTab = inx + 1;
      } else {
        nextTab = inx - 1;
      }
      state.activeTab = list[nextTab].id;
    }
    delete state.tabs[tabId];
  });

  const clearEditing = action(
    'editing.cancel',
    (resourceId: Resource['id']) => {
      console.log('Clear edt', resourceId);
      delete state.cache[resourceId];
    }
  );

  const deleteResource = action(
    'editing.delete',
    async (resourceId: Resource['id']) => {
      const resource = state.resources[resourceId];

      if (!resource?.isNew) {
        switch (resource.type) {
          case ResourceType.Template:
            await templateApi.delete({ id: resource.data.id });
            break;
          case ResourceType.Context:
            await contextApi.delete({ id: resource.data.id });
            break;
          case ResourceType.Component:
            await componentApi.delete({ id: resource.data.id });
            break;
        }
      }

      delete state.resources[resourceId];
      clearEditing(resourceId);
      const tab = tabs.get().find((x) => x.resource === resourceId);
      if (tab) {
        closeTab(tab.id);
      }
    }
  );

  const saveResources = action(
    'editing.save',
    async (...resourcesIds: Array<Resource['id']>) => {
      const resourcesForSave = resourcesIds.map((x) => state.resources[x]);
      await Promise.all(
        Object.values(ResourceType).map((x) => {
          const resourceByType = resourcesForSave.filter((r) => r.type === x);
          if (resourceByType.length === 0) {
            return Promise.resolve();
          }
          const data: any[] = resourceByType.map((rsc) => {
            const cache = state.cache[rsc.id] || {};

            return {
              ...rsc.data,
              ...cache.value,
            };
          });
          console.log('Save resource', x, JSON.stringify(data));
          switch (x) {
            case ResourceType.Template:
              return templateApi.save(data);
            case ResourceType.Context:
              return contextApi.save(data);
            case ResourceType.Component:
              return componentApi.save(data);
          }
        })
      );
      const newResources = resources.get().filter((x) => x.isNew);
      saveResourcesToLS(
        newResources.filter((x) => !resourcesIds.includes(x.id))
      );
      newResources.forEach((x) => {
        state.resources[x.id].isNew = false;
      });

      await init();
      resourcesIds.map((id) => clearEditing(id));
    }
  );

  const createValueManager = <
    TValue extends TemplateBase = TemplateBase,
    TValueAccessor extends ValueAccessor<TValue> = ValueAccessor<TValue>
  >(
    accessor: TValueAccessor
  ) => {
    const getter = computedFn(() => {
      const tab = state.tabs[state.activeTab];
      const resource = state.resources[tab.resource] as Resource<TValue>;
      const cache = state.cache[resource.id] as EditCache<TValue>;
      console.log('createValueManager get');
      return accessor.get(resource?.data, cache?.value, cache?.temp);
    });
    const setter = action((val?: string) => {
      const tab = state.tabs[state.activeTab];
      const resource = state.resources[tab.resource] as Resource<TValue>;
      const cache = state.cache[resource.id] as EditCache<TValue>;
      const setResult = accessor.set(val, cache?.value, resource?.data);
      if (accessor.equal?.(cache?.value || resource?.data, setResult.update)) {
        return;
      }
      updateResource(
        resource.id,
        resource.hash,
        setResult.update,
        setResult.temp
      );
    });
    return { get: getter, set: setter };
  };

  const getValue = <TItem extends TemplateBase, TField extends keyof TItem>(
    resource: Resource<TItem>,
    field: TField
  ): TItem[TField] | undefined => {
    const cache = state.cache[resource.id] as EditCache<TItem>;
    return cache?.value?.[field] || resource?.data?.[field];
  };

  const setValue = action(
    <TField extends keyof TItem, TItem extends TemplateBase = TemplateBase>(
      resource: Resource<TItem>,
      field: TField,
      value: TItem[TField]
    ) => {
      updateResource(resource.id, resource.hash, { [field]: value });
    }
  );

  return {
    state,
    selectors: {
      resourceByType,
      tabs: () => tabs.get(),
      current: () => current.get(),
      isChanged,
      resourceByTab,
    },
    createValueAccessor: createValueManager,
    edit: {
      get: getValue,
      set: setValue,
    },
    init,
    actions: {
      clearEditing,
      addResource,
      updateResource,
      deleteResource,
      openTab,
      closeTab,
      saveResources,
    },
  };
};

export type EditorStore = ReturnType<typeof makeEditorStore>;
