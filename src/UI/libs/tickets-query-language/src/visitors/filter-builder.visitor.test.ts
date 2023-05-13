import { TicketsQueryParser } from '../parser';
import { TicketFilterBuilderVisitor } from './ticket-filter-builder.visitor';
import { TicketFilterValue } from '@help-line/entities/client/api';
import { TicketsQueryLexer } from '../lexer';
import { findLastKey } from 'lodash';

describe('FilterBuilderVisitor', () => {
  const parser = new TicketsQueryParser();
  const lexer = new TicketsQueryLexer();

  it('should create', () => {
    expect(() => new TicketFilterBuilderVisitor()).not.toThrow();
  });

  const cases = [
    ['id=1-1111111', { $type: 'TicketIdFilter', value: '1-1111111' }],
    [
      'id=1-1111111 or id=2-1111111',
      {
        $type: 'TicketFilterGroup',
        intersection: false,
        filters: [
          { $type: 'TicketIdFilter', value: '1-1111111' },
          { $type: 'TicketIdFilter', value: '2-1111111' },
        ],
      },
    ],
    [
      'id=1-1111111 and id=2-1111111',
      {
        $type: 'TicketFilterGroup',
        intersection: true,
        filters: [
          { $type: 'TicketIdFilter', value: '1-1111111' },
          { $type: 'TicketIdFilter', value: '2-1111111' },
        ],
      },
    ],
    [
      'id=1-1111111 and (id=2-1111111 or id=3-1111111)',
      {
        $type: 'TicketFilterGroup',
        intersection: true,
        filters: [
          { $type: 'TicketIdFilter', value: '1-1111111' },
          {
            $type: 'TicketFilterGroup',
            intersection: false,
            filters: [
              { $type: 'TicketIdFilter', value: '2-1111111' },
              { $type: 'TicketIdFilter', value: '3-1111111' },
            ],
          },
        ],
      },
    ],
    [
      'id=1-1111111 or (id=2-1111111 and id=3-1111111)',
      {
        $type: 'TicketFilterGroup',
        intersection: false,
        filters: [
          { $type: 'TicketIdFilter', value: '1-1111111' },
          {
            $type: 'TicketFilterGroup',
            intersection: true,
            filters: [
              { $type: 'TicketIdFilter', value: '2-1111111' },
              { $type: 'TicketIdFilter', value: '3-1111111' },
            ],
          },
        ],
      },
    ],
    [
      'id=1-1111111 or (id=2-1111111 and (id=3-1111111 or id=4-1111111))',
      {
        $type: 'TicketFilterGroup',
        intersection: false,
        filters: [
          { $type: 'TicketIdFilter', value: '1-1111111' },
          {
            $type: 'TicketFilterGroup',
            intersection: true,
            filters: [
              { $type: 'TicketIdFilter', value: '2-1111111' },
              {
                $type: 'TicketFilterGroup',
                intersection: false,
                filters: [
                  { $type: 'TicketIdFilter', value: '3-1111111' },
                  { $type: 'TicketIdFilter', value: '4-1111111' },
                ],
              },
            ],
          },
        ],
      },
    ],
  ] as Array<[string, TicketFilterValue]>;

  it.each(cases)(`should correct parse: %s`, (query, expected) => {
    const visitor = new TicketFilterBuilderVisitor();
    const lexerResult = lexer.tokenize(query);
    parser.input = lexerResult.tokens;
    const root = parser.$expression();
    const result = visitor.visit(root);
    expect(result).toEqual(expected);
  });
});
