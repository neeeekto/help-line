import { CancellationToken, editor, languages, Position } from 'monaco-editor';
import { Monaco } from '@monaco-editor/react';
import { Component, TemplateBase } from '@help-line/entities/admin/api';
import { useRef } from 'react';
import CompletionItem = languages.CompletionItem;
import { ResourceType, useEditStore } from '../../state';

interface Info {
  src: ResourceType;
  data: TemplateBase;
}

export const useEditorSuggestions = () => {
  const store$ = useEditStore();
  const completionItemProvider = useRef<languages.CompletionItemProvider>({
    provideCompletionItems: (
      model: editor.ITextModel,
      position: Position,
      context: languages.CompletionContext,
      token: CancellationToken
    ): languages.ProviderResult<languages.CompletionList> => {
      const suggestions: CompletionItem[] = [];
      const textUntilPosition = model.getValueInRange({
        startLineNumber: position.lineNumber,
        startColumn: 1,
        endLineNumber: position.lineNumber,
        endColumn: position.column,
      });
      const word = model.getWordUntilPosition(position);
      const range = {
        startLineNumber: position.lineNumber,
        endLineNumber: position.lineNumber,
        startColumn: word.startColumn,
        endColumn: word.endColumn,
      };
      // Match handlebars opening delimiter
      if (/.*{{#?>\s?$/m.test(textUntilPosition)) {
        const components = store$.selectors.resourceByType<Component>(
          ResourceType.Component
        );
        for (const comp of components) {
          // Push handlebars snippets
          suggestions.push({
            label: comp.id,
            kind: languages.CompletionItemKind.Keyword,
            insertText: `${comp.data.id}`,
            range: range,
          });
        }
      }
      console.log(textUntilPosition, suggestions);
      return {
        suggestions,
      };
    },
  } as languages.CompletionItemProvider);

  return (editor: editor.IStandaloneCodeEditor, monaco: Monaco) => {
    monaco.languages.registerCompletionItemProvider(
      'handlebars',
      completionItemProvider.current
    );
  };
};
