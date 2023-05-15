import { useCallback, useState } from 'react';
import MonacoEditor, { Monaco } from '@monaco-editor/react';
import { editor } from 'monaco-editor';
import { TicketsQueryLanguage } from '@help-line/tickets-query-language';

const lang = new TicketsQueryLanguage({
  async getLang(): Promise<string[]> {
    return ['en', 'ru'];
  },
  async getOperators(search?: string): Promise<string[]> {
    console.log('getOperators', search);
    return ['yymoroz3@gmail.ru', 'save@test.te'];
  },
});

export const TicketQueryFilterEditor = () => {
  const [query, setQuery] = useState('');

  const stupMonaco = useCallback(
    (editor: editor.IStandaloneCodeEditor, monaco: Monaco) => {
      lang.monaco.install(editor, monaco);
    },
    [lang]
  );

  return (
    <MonacoEditor
      height="100%"
      defaultLanguage={lang.monaco.ID}
      language={lang.monaco.ID}
      value={query}
      onChange={(val) => setQuery(val!)}
      onMount={stupMonaco}
      theme={'light'}
      options={{
        language: lang.monaco.ID,
        fixedOverflowWidgets: true,
        contextmenu: false,
        wordBasedSuggestions: false,
        minimap: {
          enabled: false,
        },
        theme: 'light',
      }}
    ></MonacoEditor>
  );
};
