import { createToken, Lexer } from 'chevrotain';

export const OrToken = createToken({
  name: 'Or',
  pattern: /or|\|/,
  label: 'or',
});
export const AndToken = createToken({
  name: 'And',
  pattern: /and|&/,
  label: 'and',
});

export const LParenToken = createToken({
  name: 'LParen',
  pattern: '(',
  label: '(',
});
export const RParenToken = createToken({
  name: 'RParen',
  pattern: ')',
  label: ')',
});

export const EqualToken = createToken({
  name: 'Equal',
  pattern: '=',
  label: '=',
});
export const NotEqualToken = createToken({
  name: 'NotEqual',
  pattern: '!=',
  label: '!=',
});
export const LessToken = createToken({
  name: 'Less',
  pattern: '<',
  label: '<',
});
export const LessOrEqualToken = createToken({
  name: 'LessOrEqual',
  pattern: '<=',
  label: '<=',
});
export const GreatToken = createToken({
  name: 'Great',
  pattern: '>',
  label: '>',
});
export const GreatOrEqualToken = createToken({
  name: 'GreatOrEqual',
  pattern: '>=',
  label: '>=',
});

export const PlusToken = createToken({
  name: 'Plus',
  pattern: '+',
  label: '+',
});
export const MinusToken = createToken({
  name: 'Minus',
  pattern: '-',
  label: '-',
});

export const IntegerToken = createToken({
  name: 'Integer',
  pattern: /[1-9]\d*/,
  label: 'number',
});

export const ArrStartToken = createToken({
  name: 'ArrStart',
  pattern: '[',
  label: '[',
});
export const ArrEndToken = createToken({
  name: 'ArrEnd',
  pattern: ']',
  label: ']',
});

export const WhiteSpace = createToken({
  name: 'WhiteSpace',
  pattern: /\s+/,
  group: Lexer.SKIPPED,
});

export const DateValueToken = createToken({
  name: 'DateValue',
  pattern:
    /[0-9]{2}.[0-9]{2}.[0-9]{4} (([0-1][1-9])|(2[1-3])):([0-5][0-9]):([0-5][0-9])/,
  label: 'date: DD.MM.YYYY hh:mm:ss',
});

export const DateNowToken = createToken({
  name: 'DateNow',
  pattern: 'now',
  label: 'now',
});

export const DateDurationToken = createToken({
  name: 'DateDuration',
  pattern: /\d{1,}[hmsd]/,
  label: 'duration: 1d2h3m3s',
});

export const TicketIdFilterKeyToken = createToken({
  name: 'IdFilterKey',
  pattern: 'id',
  label: 'id',
});

export const AssigmentFilterKeyToken = createToken({
  name: 'AssigmentFilterKey',
  pattern: 'assigment',
  label: 'assigment',
});
export const AssigmentMeToken = createToken({
  name: 'AssigmentMe',
  pattern: 'me',
  label: 'me',
});
export const AssigmentNoneToken = createToken({
  name: 'AssigmentNone',
  pattern: 'none',
  label: 'none',
});
export const StringValueToken = createToken({
  name: 'StringValue',
  pattern: /[a-zA-Z]\w*/,
  label: 'string',
});

export const EmailValueToken = createToken({
  name: 'EmailValue',
  pattern: /[a-zA-Z]\w*@[a-zA-Z]\w*.[a-zA-Z]\w*/,
  label: 'string',
});

export const HasAttachmentFilterKeyToken = createToken({
  name: 'HasAttachmentFilterKey',
  pattern: 'hasAttachment',
  label: 'hasAttachment',
});

export const CreateDateFilterKeyToken = createToken({
  name: 'CreateDateFilterKey',
  pattern: 'createDate',
  label: 'createDate',
});

export const HardAssigmentFilterKeyToken = createToken({
  name: 'HardAssigmentFilterKey',
  pattern: 'hardAssigment',
  label: 'hardAssigment',
});

export const IterationCountFilterKeyToken = createToken({
  name: 'IterationCountFilterKey',
  pattern: 'iterationCount',
  label: 'iterationCount',
});

export const LanguageFilterKeyToken = createToken({
  name: 'LanguageFilterKey',
  pattern: 'language',
  label: 'language',
});

export const LastMessageFilterKeyToken = createToken({
  name: 'LastMessageFilterKey',
  pattern: 'lastMessage',
  label: 'lastMessage',
});

export const LastReplyFilterKeyToken = createToken({
  name: 'LastReplyFilterKey',
  pattern: 'lastReply',
  label: 'lastReply',
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
  pattern: /[0-9]-[0-9]{2,7}/,
  label: 'X-XXXXXXX',
});

export const SourceKeyToken = createToken({
  name: 'SourceKey',
  pattern: 'source',
});

export const PlatformsKeyToken = createToken({
  name: 'PlatformsKey',
  pattern: 'platforms',
  label: 'platforms',
});

export const ProjectFilterKeyToken = createToken({
  name: 'ProjectFilterKey',
  pattern: 'project',
  label: 'project',
});

export const StatusFilterKeyToken = createToken({
  name: 'StatusFilterKey',
  pattern: 'status',
  label: 'status',
});

export const TagFilterKeyToken = createToken({
  name: 'TagFilterKey',
  pattern: 'tags',
});

export const BooleanValueToken = createToken({
  name: 'BooleanValue',
  pattern: /true|false/,
  label: 'true/false',
});

export const DotToken = createToken({
  name: 'Dot',
  pattern: '.',
});

export const CommaToken = createToken({ name: 'Comma', pattern: /,/ });

// We need this token only for suggestion!
export const UnknownToken = createToken({
  name: 'Unknown',
  pattern: String.fromCharCode(0),
  label: 'skip', //for conditions in visit trees
});
