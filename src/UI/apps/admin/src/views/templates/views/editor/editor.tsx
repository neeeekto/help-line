import React, { useCallback, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Tabs, Typography } from 'antd';
import { editorStore } from '../../state/editor.store';
import cn from 'classnames';
import css from './editor.module.scss';
import MonacoEditor, { Monaco, OnMount } from '@monaco-editor/react';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { FullPageContainer } from '@help-line/components';
import { EditorTab } from './editor-tab';
import { EditedItem, Opened } from '../../state/editro.types';
import {
  useTemplateItemValue,
  useTemplateValueFactory,
} from '../../utils/editor.utils';
import { editor, KeyCode, KeyMod } from 'monaco-editor';
import { useSaveAll } from '../../templates.hooks';
import { useEditorSuggestions } from './editor.suggestion';

const MonacoIde: React.FC<{ item: Opened }> = observer(({ item }) => {
  const editModel = editorStore.getEditModelByOpened(item);
  const value = useTemplateItemValue(item, editModel);
  const updater = useTemplateValueFactory(item);
  const onChange = useCallback(
    (val?: string) => {
      editorStore.change(item, updater(val || ''));
    },
    [item, updater]
  );

  const saveAll = useSaveAll();

  const suggestion = useEditorSuggestions();

  const onSetup = useCallback(
    (editor: editor.IStandaloneCodeEditor, monaco: Monaco) => {
      const saveAllHandler = async () => {
        const changed = editorStore.changed.get();
        await saveAll(editorStore.changed.get());
        for (let changedElement of changed) {
          editorStore.markAsSaved(changedElement);
        }
      };
      editor.addCommand(KeyMod.Shift | KeyCode.KeyS, async () => {
        await saveAll([editModel]);
        editorStore.markAsSaved(editModel);
      });
      editor.addCommand(
        KeyMod.Shift | KeyMod.CtrlCmd | KeyCode.KeyS,
        saveAllHandler
      );
      editor.addCommand(
        KeyMod.Shift | KeyMod.WinCtrl | KeyCode.KeyS,
        saveAllHandler
      );
      suggestion(editor, monaco, () => ({
        src: editModel.src,
        data: editModel.current,
      }));
    },
    []
  );

  return (
    <MonacoEditor
      height="100%"
      defaultLanguage={item.lang}
      language={item.lang}
      value={item.value || value}
      options={{ readOnly: !!item.value }}
      onChange={onChange}
      onMount={onSetup}
    />
  );
});

export const Editor: React.FC = observer(() => {
  const active = editorStore.active.get();
  if (!active) {
    return (
      <div
        className={cn(
          boxCss.flex,
          boxCss.flexColumn,
          boxCss.fullWidth,
          boxCss.fullHeight,
          boxCss.alignItemsCenter,
          boxCss.justifyContentCenter,
          css.emptyEditor
        )}
      >
        <Typography.Text type="secondary">
          Please, select or add item...
        </Typography.Text>
      </div>
    );
  }

  return (
    <div
      className={cn(
        boxCss.flex,
        boxCss.flexColumn,
        boxCss.fullWidth,
        boxCss.fullHeight,
        boxCss.overflowHidden
      )}
    >
      <div
        className={cn(
          boxCss.flex,
          boxCss.overflowAuto,
          boxCss.flex00Auto,
          css.tabs
        )}
      >
        {editorStore.state.opened.map((x) => (
          <EditorTab key={`${x.src}_${x.id}_${x.field}`} item={x} />
        ))}
      </div>
      <FullPageContainer className={cn(css.monacoBox)}>
        <div
          className={cn(spacingCss.paddingHorizLg, spacingCss.marginBottomLg)}
        >
          <Typography.Text type="secondary">
            {active.src} / {active.id} / {active.field}
          </Typography.Text>
        </div>
        <MonacoIde item={active} />
      </FullPageContainer>
    </div>
  );
});
