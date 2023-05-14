import { languages, editor, Position, CancellationToken } from 'monaco-editor';
import { TicketsQueryLexer, tokens } from '../lexer';
import { TicketsQueryParser } from '../parser';
import { EditorState } from './editor-state';

export class TicketsQueryMonacoCompletionItemProvider
  implements languages.CompletionItemProvider
{
  triggerCharacters = [' '];

  constructor(
    private readonly lexer: TicketsQueryLexer,
    private readonly parser: TicketsQueryParser,
    private readonly state: EditorState
  ) {}

  provideCompletionItems(
    model: editor.ITextModel,
    position: Position,
    context: languages.CompletionContext,
    token: CancellationToken
  ): languages.ProviderResult<languages.CompletionList> {
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
    const syntacticSuggestions = this.parser.computeContentAssist(
      'expression',
      lexingResult.tokens
    );
    const tokenTypesSuggestions = syntacticSuggestions.map(
      (suggestion) => suggestion.nextTokenType
    );

    const actualSuggestions: languages.CompletionItem[] = [];
    const range = {
      startLineNumber: position.lineNumber,
      endLineNumber: position.lineNumber,
      startColumn: word.startColumn,
      endColumn: word.endColumn,
    };
    const addSuggestion = (text: string) => {
      actualSuggestions.push({
        range: range,
        kind: languages.CompletionItemKind.Variable,
        command: {
          id: 'editor.action.triggerSuggest',
          title: 'suggest-invoking',
        },
        insertText: text + ' ',
        label: text,
      });
    };

    console.log('provideCompletionItems', tokenTypesSuggestions);

    for (const tokenTypesSuggestion of tokenTypesSuggestions) {
      if (
        tokenTypesSuggestion == tokens.LParenToken ||
        tokenTypesSuggestion == tokens.RParenToken
      )
        continue;
      if (typeof tokenTypesSuggestion.PATTERN === 'string') {
        if (tokenTypesSuggestion.PATTERN === tokens.UnknownToken.PATTERN)
          continue;

        addSuggestion(tokenTypesSuggestion.PATTERN);
        continue;
      }

      /*if (tokenTypesSuggestion.name == 'Key') {
        const alreadyUsed = InstanceParser.keyCompletion(
          query.substring(0, wordStartOffset) +
            unknownTokenPattern +
            query.substring(wordEndOffset)
        );
        allKeys.forEach(
          (key) => !alreadyUsed.includes(key) && addSuggestion(key)
        );
        continue;
      }*/

      /*if (tokenTypesSuggestion.name == 'Value') {
        const result = await queryProvider.getValueSuggestions(
          model.getValue(),
          wordStartOffset,
          wordEndOffset
        );
        result.forEach((suggest) => suggest && addSuggestion(suggest));
      }*/
    }
    return { incomplete: true, suggestions: actualSuggestions };
  }
}
