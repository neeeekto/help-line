import { TicketsQueryParser } from '../parser';
import { TicketFilterBuilderVisitor } from './ticket-filter-builder.visitor';
import {
  TicketFilterDateValueActionOperation,
  TicketFilterOperators,
  TicketFilterValue,
} from '@help-line/entities/client/api';
import { TicketsQueryLexer } from '../lexer';

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
    [
      'createDate>now-1d and createDate<now',
      {
        $type: 'TicketFilterGroup',
        intersection: true,
        filters: [
          {
            $type: 'TicketCreateDateFilter',
            value: {
              $type: 'FilterDateValue',
              action: {
                amount: '1.0:0:0',
                operation: TicketFilterDateValueActionOperation.Sub,
              },
              dateTime: null,
              operator: TicketFilterOperators.Great,
            },
          },
          {
            $type: 'TicketCreateDateFilter',
            value: {
              $type: 'FilterDateValue',
              dateTime: null,
              operator: TicketFilterOperators.Less,
            },
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
    console.log(result);
    expect(result).toEqual(expected);
  });
});
