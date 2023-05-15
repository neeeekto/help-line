import {
  CancellationToken,
  editor,
  IRange,
  languages,
  Position,
} from 'monaco-editor';
import format from 'date-fns/format';
import { TicketsQueryLexer, tokens } from '../lexer';
import { TicketsQueryParser } from '../parser';
import { EditorState } from './editor-state';
import last from 'lodash/last';
import { ISyntacticContentAssistPath } from '@chevrotain/types';
import { ISuggestProvider } from './suggest-provider';
import CompletionItemKind = languages.CompletionItemKind;

export class TicketsQueryMonacoCompletionItemProvider
  implements languages.CompletionItemProvider
{
  constructor(
    private readonly suggestProvider: ISuggestProvider,
    private readonly lexer: TicketsQueryLexer,
    private readonly parser: TicketsQueryParser,
    private readonly state: EditorState
  ) {}

  async provideCompletionItems(
    model: editor.ITextModel,
    position: Position,
    context: languages.CompletionContext,
    token: CancellationToken
  ): Promise<languages.CompletionList> {
    const word = model.getWordUntilPosition(position);
    const wordStartOffset = model.getOffsetAt({
      column: word.startColumn,
      lineNumber: position.lineNumber,
    });

    const wordEndOffset = model.getOffsetAt({
      column: word.endColumn,
      lineNumber: position.lineNumber,
    });

    const query = model.getValue();

    const lexingResult = this.lexer.tokenize(
      query.substring(0, wordStartOffset)
    );
    const assistPaths = this.parser.computeContentAssist(
      'expression',
      lexingResult.tokens
    );

    const range = {
      startLineNumber: position.lineNumber,
      endLineNumber: position.lineNumber,
      startColumn: word.startColumn,
      endColumn: word.endColumn,
    };

    const result: languages.CompletionItem[] = [];

    for (const assistPath of assistPaths) {
      const token = assistPath.nextTokenType;
      switch (token.name) {
        case tokens.UnknownToken.name:
          continue;

        case tokens.OrToken.name:
        case tokens.AndToken.name:
          result.push(
            this.createCompletionItem(
              range,
              CompletionItemKind.Unit,
              token.LABEL!,
              `${token.LABEL} `
            )
          );
          break;

        case tokens.LParenToken.name:
        case tokens.RParenToken.name:
        case tokens.ArrStartToken.name:
        case tokens.ArrEndToken.name:
          continue;

        case tokens.IntegerToken.name:
          continue;
        case tokens.StringValueToken.name:
          const suggest = await this.tryGetSuggestionForString(
            range,
            word.word,
            assistPath
          );
          result.push(...suggest);
          break;

        case tokens.DateValueToken.name:
          const now = format(Date.now(), 'dd.MM.yyyy HH:mm:ss');
          result.push(
            this.createCompletionItem(
              range,
              CompletionItemKind.Value,
              `current: ${now}`,
              `${now} `
            )
          );
          break;

        case tokens.DateDurationToken.name:
          result.push(
            this.createCompletionItem(
              range,
              CompletionItemKind.Value,
              `1 Hour`,
              `1h`
            ),
            this.createCompletionItem(
              range,
              CompletionItemKind.Value,
              `1 Week`,
              `7d`
            ),
            this.createCompletionItem(
              range,
              CompletionItemKind.Value,
              `1 Day`,
              `1d`
            ),
            this.createCompletionItem(
              range,
              CompletionItemKind.Value,
              `1 Month`,
              `30d`
            ),
            this.createCompletionItem(
              range,
              CompletionItemKind.Value,
              `Half year`,
              `183d`
            )
          );
          break;

        case tokens.PlusToken.name:
        case tokens.MinusToken.name:
          result.push(
            this.createCompletionItem(
              range,
              CompletionItemKind.Value,
              token.LABEL!,
              `${token.LABEL} `
            )
          );
          break;

        case tokens.BooleanValueToken.name:
          result.push(
            this.createCompletionItem(
              range,
              CompletionItemKind.Value,
              'true',
              `true `
            ),
            this.createCompletionItem(
              range,
              CompletionItemKind.Value,
              'false',
              `false `
            )
          );
          break;

        case tokens.AssigmentNoneToken.name:
        case tokens.AssigmentMeToken.name:
          result.push(
            this.createCompletionItem(
              range,
              CompletionItemKind.Constant,
              token.LABEL!,
              `${token.LABEL!} `
            )
          );
          break;
        case tokens.EqualToken.name:
        case tokens.NotEqualToken.name:
        case tokens.GreatToken.name:
        case tokens.GreatOrEqualToken.name:
        case tokens.LessToken.name:
        case tokens.LessOrEqualToken.name:
          result.push(
            this.createCompletionItem(
              range,
              CompletionItemKind.Operator,
              token.LABEL!,
              `${token.LABEL} `
            )
          );
          break;

        default:
          result.push(
            this.createCompletionItem(
              range,
              CompletionItemKind.Variable,
              token.LABEL!,
              `${token.LABEL} =`
            )
          );
      }
    }
    return { incomplete: true, suggestions: result };
  }

  private async tryGetSuggestionForString(
    range: IRange,
    currentWord: string,
    assistPath: ISyntacticContentAssistPath
  ): Promise<languages.CompletionItem[]> {
    const lastRule = last(assistPath.ruleStack);
    console.log(lastRule);
    if (lastRule === 'languageFilter') {
      return this.suggestProvider
        .getLang()
        .then((res) =>
          res
            .filter((lang) => lang.startsWith(currentWord))
            .map((lang) =>
              this.createCompletionItem(
                range,
                CompletionItemKind.Value,
                lang,
                lang
              )
            )
        );
    }

    if (lastRule === 'assigmentFilterValue') {
      const operators = await this.suggestProvider.getOperators(currentWord);
      return operators.map((operator) =>
        this.createCompletionItem(
          range,
          CompletionItemKind.Value,
          operator,
          operator
        )
      );
    }

    return [];
  }

  private createCompletionItem(
    range: IRange,
    kind: languages.CompletionItemKind,
    label: string,
    insertText: string
  ) {
    return {
      range,
      kind,
      command: {
        id: 'editor.action.triggerSuggest',
        title: 'suggest-invoking',
      },
      insertText,
      label,
    };
  }
}
