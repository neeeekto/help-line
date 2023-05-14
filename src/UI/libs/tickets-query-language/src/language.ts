import { TicketsQueryLexer } from './lexer';
import { TicketsQueryParser } from './parser';
import { TicketsQueryMonacoLanguage } from './monaco';
import { TicketFilterValue } from '@help-line/entities/client/api';
import { TicketFilterBuilderVisitor } from './visitors';

export class TicketsQueryLanguage {
  public readonly lexer = new TicketsQueryLexer();
  public readonly parser = new TicketsQueryParser();
  public readonly monaco = new TicketsQueryMonacoLanguage(
    this.lexer,
    this.parser
  );

  private readonly filterBuilderVisitor = new TicketFilterBuilderVisitor();

  public parse(query: string): TicketFilterValue {
    this.parser.input = this.lexer.tokenize(query).tokens;
    return this.filterBuilderVisitor.visit(this.parser.$expression());
  }

  public stringify(value: TicketFilterValue): string {
    return '';
  }
}
