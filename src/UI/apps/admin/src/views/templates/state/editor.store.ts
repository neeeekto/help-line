import { observable, action, computed } from 'mobx';
import { EditedItem, Opened, SourceType } from './editro.types';
import { TemplateBase } from '@help-line/entities/admin/api';
import cloneDeep from 'lodash/cloneDeep';

export const makeEditorStore = () => {
  const state = observable({
    edited: [] as EditedItem[],
    opened: [] as Opened[],
  });

  const getEditModelByOpened = (openModel: Opened) => {
    const edit = state.edited.find(
      (x) => x.id === openModel.id && x.src === openModel.src
    );
    if (!edit) {
      throw new Error(
        `Edit model not found! Tab '${openModel.id}.${openModel.src}:${openModel.field}'`
      );
    }

    return edit;
  };

  const isEqual = (
    x: Pick<EditedItem, 'id' | 'src'>,
    y: Pick<EditedItem, 'id' | 'src'>
  ) => x.src == y.src && x.id === y.id;

  const active = computed(() => {
    return state.opened.find((x) => x.active);
  });

  const changed = computed(() => {
    return state.edited.filter(
      (x) => x.current?.updatedAt !== x.original?.updatedAt
    );
  });

  const open = action(
    'editing.open',
    (item: EditedItem, field: string, lang: string, value?: string) => {
      const editModel = state.edited.find((x) => isEqual(x, item));
      if (!editModel) {
        state.edited.push({ ...item });
      }
      state.opened.forEach((x) => (x.active = false));
      let openModel = state.opened.find(
        (x) => x.id === item.id && x.src === item.src && x.field === field
      );
      if (!openModel) {
        openModel = observable({
          id: item.id,
          src: item.src,
          active: true,
          field,
          lang,
        });
        state.opened.push(openModel);
      }
      openModel.active = true;
      if (value) {
        openModel.value = value;
      }
    }
  );

  const openTemplateItem = action(
    'editing.open.templateItem',
    (item: TemplateBase, src: SourceType, field: string, lang: string) => {
      open(
        {
          id: item.id,
          src,
          current: cloneDeep(item),
          original: item,
        },
        field,
        lang
      );
    }
  );

  const focus = action('editing.focus', (opened: Opened) => {
    state.opened.forEach((x) => (x.active = false));
    opened.active = true;
  });

  const changeField = action(
    'editing.change',
    (opened: Opened, field: string, newValue: any) => {
      const editModel = getEditModelByOpened(opened);
      (editModel.current as any)[field] = newValue;
      editModel.current.updatedAt = new Date(Date.now()).toISOString();
    }
  );

  const change = action('editing.change', (opened: Opened, newValue: any) => {
    changeField(opened, opened.field, newValue);
  });

  const remove = action('editing.remove', (id: string, src: SourceType) => {
    const openModels = state.opened.filter((x) => x.src === src && x.id === id);
    state.edited = state.edited.filter((x) => !(x.id === id && x.src === src));
    for (let openModel of openModels) {
      close(openModel);
    }
  });

  const close = action('editing.close', (opened: Opened) => {
    const inx = state.opened.findIndex(
      (x) => isEqual(x, opened) && x.field === opened.field
    );
    if (inx === -1) {
      console.warn(
        `Tab '${opened.id}.${opened.src}:${opened.field}' is not exist!`
      );
      return;
    }

    if (opened.active) {
      state.opened.forEach((x) => (x.active = false));
      if (state.opened.length > 1) {
        if (inx === 0) {
          state.opened[1].active = true;
        } else if (inx === state.opened.length - 1) {
          state.opened[state.opened.length - 2].active = true;
        } else {
          state.opened[inx - 1].active = true;
        }
      }
    }
    state.opened = state.opened.filter((x) => !isEqual(x, opened));
  });

  const markAsSaved = action('editing.save', (editModel: EditedItem) => {
    editModel.original = { ...editModel.current };
  });

  const reset = action(
    'editing.save',
    (id: string, src: SourceType, data: TemplateBase) => {
      const editModel = state.edited.find((x) => x.id === id && x.src === src);
      if (editModel) {
        editModel.current = observable(cloneDeep(data));
      }
    }
  );

  return {
    state,
    open,
    focus,
    change,
    close,
    remove,
    isEqual,
    active,
    getEditModelByOpened,
    markAsSaved,
    openTemplateItem,
    changeField,
    changed,
    reset,
  };
};

export const editorStore = makeEditorStore();
