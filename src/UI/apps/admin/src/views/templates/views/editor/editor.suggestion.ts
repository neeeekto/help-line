import {
  TemplateItemQueries,
  useComponentQueries,
  useContextQueries,
  useTemplatesQueries,
} from "@entities/templates/queries";
import { CancellationToken, editor, languages, Position } from "monaco-editor";
import { Monaco } from "@monaco-editor/react";
import { SourceType } from "@views/templates/state/editro.types";
import { TemplateItem } from "@entities/templates";
import { useRef } from "react";
import CompletionItem = languages.CompletionItem;

interface Info {
  src: SourceType;
  data: TemplateItem;
}

export const useEditorSuggestions = () => {
  const templatesQuery = useTemplatesQueries().useListQuery();
  const componentsQuery = useComponentQueries().useListQuery();
  const contextQuery = useContextQueries().useListQuery();
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
      console.log(word, textUntilPosition);
      // Match handlebars opening delimiter
      const rootScopes = ["ctx", "props", "data"];
      if (textUntilPosition.match(/.*{{$/m)) {
        suggestions.push(
          ...rootScopes.map((x) => ({
            label: x,
            kind: languages.CompletionItemKind.Field,
            insertText: `${x}.`,
            range: range,
          }))
        );
      }
      return {
        suggestions,
      };
    },
  } as languages.CompletionItemProvider);

  return (
    editor: editor.IStandaloneCodeEditor,
    monaco: Monaco,
    currentGetter: () => Info
  ) => {
    monaco.languages.registerCompletionItemProvider(
      "handlebars",
      completionItemProvider.current
    );
  };
};
