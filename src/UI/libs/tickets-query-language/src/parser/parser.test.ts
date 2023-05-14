import { TicketsQueryParser } from './parser';
import { TicketsQueryLexer } from '../lexer';

describe('TicketsQueryParser', () => {
  const lexer = new TicketsQueryLexer();
  it('should create', () => {
    expect(() => new TicketsQueryParser()).not.toThrow();
  });

  const cases = [
    'assigment=me',
    '(id=1-1111111) ',
    'id=1-1111111 and id=1-1111111',
    '(id=1-1111111 or id=1-1111111)',
    'id=1-1111111 or (id=1-1111111 or id=1-1111111)',
    'id=1-1111111 and (id=1-1111111 or id=1-1111111)',
    'id=1-1111111 or (id=1-1111111 and id=1-1111111)',
    'id=1-1111111 or id=1-1111111 and id=1-1111111',
    'id=1-1111111 and id=2-1111111 or id=3-1111111',
    'createDate=now+1h2s',
    'createDate=now+1h2s and id=2-1111111',
    'createDate=now+1h2s or id=2-1111111',
    'createDate>now-1d and createDate<now',
    'iterationCount>2 and createDate<now',
    'language=en',
    'language=[en, ru]',
  ];

  cases.forEach((testCase) => {
    it(`should correct parse: ${testCase}`, () => {
      const parser = new TicketsQueryParser();
      parser.input = lexer.tokenize(testCase).tokens;
      const parserResult = parser.$expression(); // Run parsing
      expect(parser.errors).toHaveLength(0);
    });
  });
});
