import { CstParser, EOF } from 'chevrotain';
import { TicketsQueryLexer, tokens } from '../lexer';

export class TicketsQueryParser extends CstParser {
  protected operator = this.RULE('operator', () => {
    this.OR([
      {
        ALT: () => this.CONSUME(tokens.EqualToken),
      },
      {
        ALT: () => this.CONSUME(tokens.LessToken),
      },
      { ALT: () => this.CONSUME(tokens.LessOrEqualToken) },
      { ALT: () => this.CONSUME(tokens.GreatToken) },
      { ALT: () => this.CONSUME(tokens.GreatOrEqualToken) },
      { ALT: () => this.CONSUME(tokens.NotEqualToken) },
    ]);
  });

  protected dateFilterValue = this.RULE('dateFilterValue', () => {
    this.SUBRULE(this.operator);
    this.OR([
      {
        ALT: () => this.CONSUME(tokens.DateNowToken),
      },
      {
        ALT: () => this.CONSUME(tokens.DateValueToken),
      },
      { ALT: () => this.CONSUME(tokens.UnknownToken) },
    ]);
    this.OPTION(() => {
      this.OR1([
        { ALT: () => this.CONSUME(tokens.PlusToken) },
        { ALT: () => this.CONSUME(tokens.MinusToken) },
      ]);
      this.AT_LEAST_ONE(() => {
        this.CONSUME(tokens.DateDurationToken);
      });
    });
  });

  protected assigmentFilterValue = this.RULE('assigmentFilterValue', () => {
    this.OR([
      {
        ALT: () => {
          this.CONSUME(tokens.AssigmentNoneToken);
        },
      },
      {
        ALT: () => {
          this.CONSUME(tokens.AssigmentMeToken);
        },
      },
      {
        ALT: () => {
          this.CONSUME(tokens.StringValueToken);
        },
      },
    ]);
  });

  protected idFilter = this.RULE('idFilter', () => {
    this.CONSUME(tokens.TicketIdFilterKeyToken);
    this.CONSUME(tokens.EqualToken);
    this.OR([
      { ALT: () => this.CONSUME(tokens.TicketNumberToken) },
      { ALT: () => this.CONSUME(tokens.UnknownToken) },
    ]);
  });

  protected assigmentFilter = this.RULE('assigmentFilter', () => {
    this.CONSUME(tokens.AssigmentFilterKeyToken);
    this.CONSUME(tokens.EqualToken);
    this.SUBRULE(this.assigmentFilterValue);
  });

  protected hasAttachmentFilter = this.RULE('hasAttachmentFilter', () => {
    this.CONSUME(tokens.HasAttachmentFilterKeyToken);
    this.CONSUME(tokens.EqualToken);
    this.CONSUME(tokens.BooleanValueToken);
  });

  protected createDateFilter = this.RULE('createDateFilter', () => {
    this.CONSUME(tokens.CreateDateFilterKeyToken);
    this.SUBRULE(this.dateFilterValue);
  });

  protected hardAssigmentFilter = this.RULE('hardAssigmentFilter', () => {
    this.CONSUME(tokens.HardAssigmentFilterKeyToken);
    this.CONSUME(tokens.EqualToken);
    this.CONSUME(tokens.BooleanValueToken);
  });

  protected iterationCountFilter = this.RULE('iterationCountFilter', () => {
    this.CONSUME(tokens.IterationCountFilterKeyToken);
    this.SUBRULE(this.operator);
    this.CONSUME(tokens.IntegerToken);
  });

  protected languageFilter = this.RULE('languageFilter', () => {
    this.CONSUME(tokens.LanguageFilterKeyToken);
    this.CONSUME2(tokens.EqualToken);
    this.OR([
      {
        ALT: () => this.CONSUME3(tokens.StringValueToken),
      },
      {
        ALT: () => {
          this.CONSUME3(tokens.ArrStartToken);
          this.MANY_SEP({
            SEP: tokens.CommaToken,
            DEF: () => this.CONSUME4(tokens.StringValueToken),
          });
          this.CONSUME5(tokens.ArrEndToken);
        },
      },
    ]);
  });

  protected lastMessageTypeFilter = this.RULE('lastMessageTypeFilter', () => {
    this.CONSUME(tokens.LastMessageFilterKeyToken);
    this.CONSUME(tokens.EqualToken);
    this.CONSUME(tokens.StringValueToken);
  });

  protected lastReplyFilter = this.RULE('lastReplyFilter', () => {
    this.CONSUME(tokens.LastReplyFilterKeyToken);
    this.SUBRULE(this.operator);
    this.SUBRULE(this.dateFilterValue);
  });

  // =======

  protected filter = this.RULE('filter', () => {
    this.OR([
      {
        ALT: () => this.SUBRULE(this.idFilter, { LABEL: 'Value' }),
      },
      {
        ALT: () => this.SUBRULE(this.createDateFilter, { LABEL: 'Value' }),
      },

      {
        ALT: () => this.SUBRULE(this.assigmentFilter, { LABEL: 'Value' }),
      },
      {
        ALT: () => this.SUBRULE(this.hasAttachmentFilter, { LABEL: 'Value' }),
      },

      {
        ALT: () => this.SUBRULE(this.hardAssigmentFilter, { LABEL: 'Value' }),
      },
      {
        ALT: () => this.SUBRULE(this.iterationCountFilter, { LABEL: 'Value' }),
      },
      { ALT: () => this.SUBRULE(this.languageFilter, { LABEL: 'Value' }) },
      {
        ALT: () => this.SUBRULE(this.lastMessageTypeFilter, { LABEL: 'Value' }),
      },
      { ALT: () => this.SUBRULE(this.lastReplyFilter, { LABEL: 'Value' }) },
    ]);
  });

  protected atomicExpression = this.RULE('atomicExpression', () => {
    this.OR([
      { ALT: () => this.SUBRULE(this.parenthesisExpression) },
      { ALT: () => this.SUBRULE(this.filter) },
      { ALT: () => this.CONSUME(tokens.UnknownToken) },
    ]);
  });

  protected parenthesisExpression = this.RULE('parenthesisExpression', () => {
    this.CONSUME(tokens.LParenToken);
    this.SUBRULE(this.andExpression);
    this.CONSUME(tokens.RParenToken);
  });

  // X & X & X
  protected andExpression = this.RULE('andExpression', () => {
    this.SUBRULE(this.orExpression, { LABEL: 'Left' });
    this.MANY(() => {
      this.CONSUME(tokens.AndToken);
      this.SUBRULE2(this.orExpression, { LABEL: 'Right' });
    });
  });

  // X or X or X
  protected orExpression = this.RULE('orExpression', () => {
    this.SUBRULE(this.atomicExpression, { LABEL: 'Left' });
    this.MANY(() => {
      this.CONSUME(tokens.OrToken);
      this.SUBRULE2(this.atomicExpression, { LABEL: 'Right' });
    });
  });

  $expression = this.RULE('expression', () => {
    this.SUBRULE(this.andExpression);
  });

  constructor() {
    super(TicketsQueryLexer.allTokens);
    this.performSelfAnalysis();
  }
}
