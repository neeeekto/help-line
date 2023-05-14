import { TicketsQueryLexer } from './lexer';
import * as tokens from './tokens';
import { TokenType } from 'chevrotain';

describe('TicketsQueryLexer', () => {
  const expectTokens = (query: string, target: TokenType[]) => {
    const lexer = new TicketsQueryLexer();
    const result = lexer.tokenize(query);
    expect(result.tokens.map((x) => x.tokenType.name)).toEqual(
      target.map((x) => x.name)
    );
  };

  it('should create', () => {
    expect(() => new TicketsQueryLexer()).not.toThrow();
  });

  describe('cases', () => {
    const cases = {
      'id=1-1111111': [
        tokens.TicketIdFilterKeyToken,
        tokens.EqualToken,
        tokens.TicketNumberToken,
      ],

      'assigment=none': [
        tokens.AssigmentFilterKeyToken,
        tokens.EqualToken,
        tokens.AssigmentNoneToken,
      ],
      'assigment=me': [
        tokens.AssigmentFilterKeyToken,
        tokens.EqualToken,
        tokens.AssigmentMeToken,
      ],
      'assigment=[me, none, username]': [
        tokens.AssigmentFilterKeyToken,
        tokens.EqualToken,
        tokens.ArrStartToken,
        tokens.AssigmentMeToken,
        tokens.CommaToken,
        tokens.AssigmentNoneToken,
        tokens.CommaToken,
        tokens.StringValueToken,
        tokens.ArrEndToken,
      ],
      'tags=[tag1]': [
        tokens.TagFilterKeyToken,
        tokens.EqualToken,
        tokens.ArrStartToken,
        tokens.StringValueToken,
        tokens.ArrEndToken,
      ],
      'hasAttachment=true': [
        tokens.HasAttachmentFilterKeyToken,
        tokens.EqualToken,
        tokens.BooleanValueToken,
      ],
      'hasAttachment=false': [
        tokens.HasAttachmentFilterKeyToken,
        tokens.EqualToken,
        tokens.BooleanValueToken,
      ],
      'createDate=now+1h': [
        tokens.CreateDateFilterKeyToken,
        tokens.EqualToken,
        tokens.DateNowToken,
        tokens.PlusToken,
        tokens.DateDurationToken,
      ],
      'createDate=now-1h': [
        tokens.CreateDateFilterKeyToken,
        tokens.EqualToken,
        tokens.DateNowToken,
        tokens.MinusToken,
        tokens.DateDurationToken,
      ],
      'createDate=now+1h2s': [
        tokens.CreateDateFilterKeyToken,
        tokens.EqualToken,
        tokens.DateNowToken,
        tokens.PlusToken,
        tokens.DateDurationToken,
        tokens.DateDurationToken,
      ],

      'createDate=11.11.2022 15:11:11': [
        tokens.CreateDateFilterKeyToken,
        tokens.EqualToken,
        tokens.DateValueToken,
      ],

      'status=val': [
        tokens.StatusFilterKeyToken,
        tokens.EqualToken,
        tokens.StringValueToken,
      ],
      'status=val.val': [
        tokens.StatusFilterKeyToken,
        tokens.EqualToken,
        tokens.StringValueToken,
        tokens.DotToken,
        tokens.StringValueToken,
      ],
      'status=val.[val, val]': [
        tokens.StatusFilterKeyToken,
        tokens.EqualToken,
        tokens.StringValueToken,
        tokens.DotToken,
        tokens.ArrStartToken,
        tokens.StringValueToken,
        tokens.CommaToken,
        tokens.StringValueToken,
        tokens.ArrEndToken,
      ],
      'tags=[val, val]': [
        tokens.TagFilterKeyToken,
        tokens.EqualToken,
        tokens.ArrStartToken,
        tokens.StringValueToken,
        tokens.CommaToken,
        tokens.StringValueToken,
        tokens.ArrEndToken,
      ],
      'tags!=[val, val]': [
        tokens.TagFilterKeyToken,
        tokens.NotEqualToken,
        tokens.ArrStartToken,
        tokens.StringValueToken,
        tokens.CommaToken,
        tokens.StringValueToken,
        tokens.ArrEndToken,
      ],
      'id=1-1111111 status=val (assigment=me | assigment=none)': [
        tokens.TicketIdFilterKeyToken,
        tokens.EqualToken,
        tokens.TicketNumberToken,
        tokens.StatusFilterKeyToken,
        tokens.EqualToken,
        tokens.StringValueToken,
        tokens.LParenToken,
        tokens.AssigmentFilterKeyToken,
        tokens.EqualToken,
        tokens.AssigmentMeToken,
        tokens.OrToken,
        tokens.AssigmentFilterKeyToken,
        tokens.EqualToken,
        tokens.AssigmentNoneToken,
        tokens.RParenToken,
      ],
      'id=????': [tokens.TicketIdFilterKeyToken, tokens.EqualToken],
      'id=1 & id=2': [
        tokens.TicketIdFilterKeyToken,
        tokens.EqualToken,
        tokens.IntegerToken,
        tokens.AndToken,
        tokens.TicketIdFilterKeyToken,
        tokens.EqualToken,
        tokens.IntegerToken,
      ],
      'id=1 | id=2': [
        tokens.TicketIdFilterKeyToken,
        tokens.EqualToken,
        tokens.IntegerToken,
        tokens.OrToken,
        tokens.TicketIdFilterKeyToken,
        tokens.EqualToken,
        tokens.IntegerToken,
      ],
      '(& &) | (& &)': [
        tokens.LParenToken,
        tokens.AndToken,
        tokens.AndToken,
        tokens.RParenToken,
        tokens.OrToken,
        tokens.LParenToken,
        tokens.AndToken,
        tokens.AndToken,
        tokens.RParenToken,
      ],
      '(and and) or (and and)': [
        tokens.LParenToken,
        tokens.AndToken,
        tokens.AndToken,
        tokens.RParenToken,
        tokens.OrToken,
        tokens.LParenToken,
        tokens.AndToken,
        tokens.AndToken,
        tokens.RParenToken,
      ],
    } as Record<string, TokenType[]>;

    Object.entries(cases).forEach(([query, expect]) => {
      it(`should correct tokenize for: ${query}`, () => {
        expectTokens(query, expect);
      });
    });
  });
});
