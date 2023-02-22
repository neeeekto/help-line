import React, { useCallback, useMemo } from 'react';
import { observer } from 'mobx-react-lite';
import { Typography } from 'antd';
import cn from 'classnames';

import css from './editor.module.scss';
import MonacoEditor, { Monaco } from '@monaco-editor/react';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { FullPageContainer } from '@help-line/components';
import { EditorTab } from './editor-tab';
import { EditTab, Resource, useEditStore } from '../../state';
import { editor, KeyCode, KeyMod } from 'monaco-editor';
import { useEditorSuggestions } from './editor.suggestion';
import { useSaveResourceMutation } from '../../state/hooks';

const MonacoIde: React.FC<{ tab: EditTab; resource: Resource }> = observer(
  ({ tab, resource }) => {
    const store$ = useEditStore();
    const saveMutation = useSaveResourceMutation(resource.id);
    const accessor = useMemo(
      () => store$.createValueAccessor(tab.value),
      [store$, tab.value]
    );
    const setupSuggestion = useEditorSuggestions();
    const onSetup = useCallback(
      (editor: editor.IStandaloneCodeEditor, monaco: Monaco) => {
        editor.addCommand(KeyMod.Shift | KeyCode.KeyS, async () => {
          saveMutation.mutate();
        });
        setupSuggestion(editor, monaco);
      },
      []
    );

    return (
      <MonacoEditor
        height="100%"
        defaultLanguage={tab.language}
        language={tab.language}
        value={accessor.get()}
        options={{
          readOnly: tab.readonly,
          minimap: {
            enabled: false,
          },
        }}
        onChange={accessor.set}
        onMount={onSetup}
      />
    );
  }
);

export const Editor: React.FC = observer(() => {
  const store$ = useEditStore();
  const current = store$.selectors.current();
  const tabs = store$.selectors.tabs();
  if (!current) {
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
        {tabs.map((x) => (
          <EditorTab key={x.id} tab={x} />
        ))}
      </div>
      <FullPageContainer className={cn(css.monacoBox)}>
        <div
          className={cn(spacingCss.paddingHorizLg, spacingCss.marginBottomLg)}
        >
          <Typography.Text type="secondary">
            {current.tab.breadcrumb.join(' / ')}
          </Typography.Text>
        </div>
        <MonacoIde
          key={current.tab.id}
          tab={current.tab}
          resource={current.resource}
        />
      </FullPageContainer>
    </div>
  );
});
