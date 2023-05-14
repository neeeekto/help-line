import type { IDisposable, languages } from 'monaco-editor';
import { tokens } from '../lexer';
import { TicketsQueryLexer } from '../lexer';
import { TicketsQueryParser } from '../parser';
import type * as monaco from 'monaco-editor';
import { TicketsQueryMonacoTokensProvider } from './tokens-provider';
import { EditorState } from './editor-state';
import { editor } from 'monaco-editor';
import IStandaloneCodeEditor = editor.IStandaloneCodeEditor;
import { TextModelValidator } from './text-model-validator';
import { TicketsQueryMonacoCompletionItemProvider } from './completion-item-provider';

type Monaco = typeof monaco;
export class TicketsQueryMonacoLanguage implements IDisposable {
  public static readonly ID = 'TicketQueryLanguage';
  private resources: IDisposable[] = [];

  get ID() {
    return TicketsQueryMonacoLanguage.ID;
  }

  private readonly state = new EditorState(this.lexer, this.parser);
  private readonly validator = new TextModelValidator(this.state);
  constructor(
    private readonly lexer: TicketsQueryLexer,
    private readonly parser: TicketsQueryParser
  ) {}

  public install(
    codeEditor: IStandaloneCodeEditor,
    monaco: Monaco,
    theme: 'light' | 'dark' = 'light'
  ) {
    this.installLang(monaco);

    const model = codeEditor.getModel()!;
    this.state.init(model);
    this.resources.push(
      this.installLangConf(monaco),
      this.installTokenProvider(monaco),
      this.installCompletionProvider(monaco),
      model.onDidChangeContent(() => {
        this.state.update(model);
        this.validator.validate(monaco, model);
      })
    );
  }

  private installLang(monaco: Monaco) {
    if (!monaco.languages.getLanguages().find((v) => v.id === this.ID)) {
      monaco.languages.register({ id: this.ID });
    }
  }

  private installLangConf(monaco: Monaco) {
    return monaco.languages.setLanguageConfiguration(
      TicketsQueryMonacoLanguage.ID,
      {
        autoClosingPairs: [
          { open: '(', close: ')' },
          { open: '[', close: ']' },
        ],
        surroundingPairs: [
          { open: '(', close: ')' },
          { open: '[', close: ']' },
        ],
        brackets: [
          ['(', ')'],
          ['[', ']'],
        ],
      }
    );
  }

  private installTokenProvider(monaco: Monaco) {
    return monaco.languages.setTokensProvider(
      TicketsQueryMonacoLanguage.ID,
      new TicketsQueryMonacoTokensProvider(this.lexer, this.state)
    );
  }

  private installCompletionProvider(monaco: Monaco) {
    return monaco.languages.registerCompletionItemProvider(
      TicketsQueryMonacoLanguage.ID,
      new TicketsQueryMonacoCompletionItemProvider(
        this.lexer,
        this.parser,
        this.state
      )
    );
  }
  dispose(): void {
    for (let resource of this.resources) {
      resource.dispose();
    }
    this.resources = [];
  }
}
