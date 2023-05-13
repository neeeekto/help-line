import { createToken, Lexer } from 'chevrotain';

export const OrToken = createToken({ name: 'Or', pattern: /or|\|/ });
export const AndToken = createToken({ name: 'And', pattern: /and|&/ });

export const LParenToken = createToken({
  name: 'LParen',
  pattern: '(',
});
export const RParenToken = createToken({ name: 'RParen', pattern: ')' });

export const EqualToken = createToken({ name: 'Equal', pattern: '=' });
export const NotEqualToken = createToken({ name: 'NotEqual', pattern: '!=' });
export const LessToken = createToken({ name: 'Less', pattern: '<' });
export const LessOrEqualToken = createToken({
  name: 'LessOrEqual',
  pattern: '<=',
});
export const GreatToken = createToken({ name: 'Great', pattern: '>' });
export const GreatOrEqualToken = createToken({
  name: 'GreatOrEqual',
  pattern: '>=',
});

export const PlusToken = createToken({ name: 'Plus', pattern: '+' });
export const MinusToken = createToken({ name: 'Minus', pattern: '-' });

export const IntegerToken = createToken({
  name: 'Integer',
  pattern: /[1-9]\d*/,
});

export const ArrStartToken = createToken({ name: 'ArrStart', pattern: '[' });
export const ArrEndToken = createToken({ name: 'ArrEnd', pattern: ']' });

export const WhiteSpace = createToken({
  name: 'WhiteSpace',
  pattern: /\s+/,
  group: Lexer.SKIPPED,
});

export const DateValueToken = createToken({
  name: 'DateValue',
  pattern:
    /[0-9]{2}.[0-9]{2}.[0-9]{4} (([0-1][1-9])|(2[1-3])):([0-5][0-9]):([0-5][0-9])/,
});

export const DateNowToken = createToken({
  name: 'DateNow',
  pattern: 'now',
});

export const DateKindToken = createToken({
  name: 'DateKind',
  pattern: /[hmsD]/,
});

export const TicketIdFilterKeyToken = createToken({
  name: 'IdFilterKey',
  pattern: 'id',
});

export const AssigmentFilterKeyToken = createToken({
  name: 'AssigmentFilterKey',
  pattern: 'assigment',
});
export const AssigmentMeToken = createToken({
  name: 'AssigmentMe',
  pattern: 'me',
});
export const AssigmentNoneToken = createToken({
  name: 'AssigmentNone',
  pattern: 'none',
});
export const StringValueToken = createToken({
  name: 'StringValue',
  pattern: /[a-zA-Z]\w*/,
});

export const HasAttachmentFilterKeyToken = createToken({
  name: 'HasAttachmentFilterKey',
  pattern: 'hasAttachment',
});

export const CreateDateFilterKeyToken = createToken({
  name: 'CreateDateFilterKey',
  pattern: 'createDate',
});

export const HardAssigmentFilterKeyToken = createToken({
  name: 'HardAssigmentFilterKey',
  pattern: 'hardAssigment',
});

export const IterationCountFilterKeyToken = createToken({
  name: 'IterationCountFilterKey',
  pattern: 'iterationCount',
});

export const LanguageFilterKeyToken = createToken({
  name: 'LanguageFilterKey',
  pattern: 'language',
});

export const LastMessageFilterKeyToken = createToken({
  name: 'LastMessageFilterKey',
  pattern: 'lastMessage',
});

export const LastReplyFilterKeyToken = createToken({
  name: 'LastReplyFilterKey',
  pattern: 'lastReply',
});

export const MetaFilterKeyToken = createToken({
  name: 'MetaFilterKey',
  pattern: 'meta',
});

export const FromTicketKeyToken = createToken({
  name: 'FromTicketKey',
  pattern: 'fromTicket',
});

export const TicketNumberToken = createToken({
  name: 'TicketNumber',
  pattern: /[0-9]-[0-9]{7}/,
});

export const SourceKeyToken = createToken({
  name: 'SourceKey',
  pattern: 'source',
});

export const PlatformsKeyToken = createToken({
  name: 'PlatformsKey',
  pattern: 'platforms',
});

export const ProjectFilterKeyToken = createToken({
  name: 'ProjectFilterKey',
  pattern: 'project',
});

export const StatusFilterKeyToken = createToken({
  name: 'StatusFilterKey',
  pattern: 'status',
});

export const TagFilterKeyToken = createToken({
  name: 'TagFilterKey',
  pattern: 'tags',
});

export const BooleanValueToken = createToken({
  name: 'BooleanValue',
  pattern: /true|false/,
});

export const DotToken = createToken({
  name: 'Dot',
  pattern: '.',
});

export const CommaToken = createToken({ name: 'Comma', pattern: /,/ });
export const UnknownToken = createToken({
  name: 'Unknown',
  pattern: String.fromCharCode(0),
  label: 'skip', //for conditions in visit trees
});
