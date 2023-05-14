import { languages } from 'monaco-editor';
import TokensProvider = languages.TokensProvider;
import { TicketsQueryLexer, tokens } from '../lexer';
import { EditorState } from './editor-state';

class TicketsQueryMonacoTokensProviderState implements languages.IState {
  clone(): languages.IState {
    return new TicketsQueryMonacoTokensProviderState();
  }

  equals(other: languages.IState): boolean {
    return true;
  }
}

export class TicketsQueryMonacoTokensProvider
  implements languages.TokensProvider
{
  constructor(
    private readonly lexer: TicketsQueryLexer,
    private readonly editorState: EditorState
  ) {}
  getInitialState(): languages.IState {
    return new TicketsQueryMonacoTokensProviderState();
  }

  tokenize(line: string, state: languages.IState): languages.ILineTokens {
    const lexingResult = this.lexer.tokenize(line);
    const tokens = lexingResult.tokens.map((token) => ({
      startIndex: token.startColumn! - 1,
      scopes: this.mapTokenNameToVSThemeToken(token.tokenType.name),
    }));
    return {
      tokens,
      endState: state.clone(),
    };
  }

  private mapTokenNameToVSThemeToken(tokenName: string) {
    switch (tokenName) {
      case tokens.OrToken.name:
      case tokens.AndToken.name:
        return 'keyword.flow';

      case tokens.TicketIdFilterKeyToken.name:
      case tokens.CreateDateFilterKeyToken.name:
      case tokens.LastMessageFilterKeyToken.name:
      case tokens.LanguageFilterKeyToken.name:
      case tokens.AssigmentFilterKeyToken.name:
      case tokens.LastReplyFilterKeyToken.name:
      case tokens.IterationCountFilterKeyToken.name:
      case tokens.HasAttachmentFilterKeyToken.name:
      case tokens.HardAssigmentFilterKeyToken.name:
      case tokens.TagFilterKeyToken.name:
        return 'keyword';

      case tokens.ArrStartToken.name:
      case tokens.ArrEndToken.name:
        return '';

      case tokens.LParenToken.name:
      case tokens.RParenToken.name:
        return '';

      case tokens.TicketNumberToken.name:
      case tokens.IntegerToken.name:
        return 'number';

      case tokens.StringValueToken.name:
        return 'string';

      case tokens.DateDurationToken.name:
        return 'keyword.json';

      default:
        return '';
    }
  }
}
