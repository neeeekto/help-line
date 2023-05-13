import { CstNode, IToken } from 'chevrotain';
import { HardAssigmentFilterKeyToken } from '../lexer/tokens';

export interface DateAmountNode extends CstNode {
  name: 'dateAmount';
  children: {
    Integer?: IToken[];
    DateKind?: IToken[];
  };
}

export interface DateFilterValueNode extends CstNode {
  name: 'dateFilterValue';
  children: {
    operator: OperatorNode[];
    DateNow?: IToken[];
    dateValue?: IToken[];
    Unknown?: IToken[];
    Plus?: IToken[];
    Minus?: IToken[];
    dateAmount?: DateAmountNode[];
  };
}

export interface OperatorNode extends CstNode {
  name: 'operator';
  children: {
    Equal?: IToken[];
    NotEqual?: IToken[];
    Less?: IToken[];
    LessOrEqual?: IToken[];
    Great?: IToken[];
    GreatOrEqual?: IToken[];
  };
}

export interface IdFilterNode extends CstNode {
  name: 'idFilter';
  children: {
    IdFilterKey: IToken[];
    Equal: IToken[];
    TicketNumber: IToken[];
  };
}

export interface CreateDateFilterNode extends CstNode {
  name: 'createDateFilter';
  children: {
    CreateDateFilterKey: IToken[];
    dateFilterValue: DateFilterValueNode[];
  };
}

export interface AssigmentFilterValueNode extends CstNode {
  name: 'assigmentFilterValue';
  children: {
    AssigmentNone?: IToken[];
    AssigmentMe?: IToken[];
    StringValue?: IToken[];
  };
}

export interface AssigmentFilterNode extends CstNode {
  name: 'assigmentFilter';
  children: {
    AssigmentFilterKey: IToken[];
    Equal: IToken[];
    assigmentFilterValue: AssigmentFilterValueNode[];
  };
}

export interface HasAttachmentFilterNode extends CstNode {
  name: 'hasAttachmentFilter';
  children: {
    HasAttachmentFilterKey: IToken[];
    Equal: IToken[];
    BooleanValue: IToken[];
  };
}

export interface HardAssigmentFilterNode extends CstNode {
  name: 'hardAssigmentFilter';
  children: {
    HardAssigmentFilterKey: IToken[];
    Equal: IToken[];
    BooleanValue: IToken[];
  };
}

export interface IterationCountFilterNode extends CstNode {
  name: 'iterationCountFilter';
  children: {
    IterationCountFilterKey: IToken[];
    operator: OperatorNode[];
    Integer: IToken[];
  };
}

export interface LanguageFilterNode extends CstNode {
  name: 'languageFilter';
  children: {
    LanguageFilterKey: IToken[];
    Equal: IToken[];
    StringValue: IToken[];
  };
}

export interface LastMessageTypeFilterNode extends CstNode {
  name: 'lastMessageTypeFilter';
  children: {
    LastMessageFilterKey: IToken[];
    Equal: IToken[];
    StringValue: IToken[];
  };
}

export interface LastReplyFilterNode extends CstNode {
  name: 'lastReplyFilter';
  children: {
    LastReplyFilterKey: IToken[];
    operator: OperatorNode[];
    dateFilterValue: DateFilterValueNode[];
  };
}

export interface FilterNode extends CstNode {
  name: 'filter';
  children: {
    Value: Array<
      | IdFilterNode
      | CreateDateFilterNode
      | AssigmentFilterNode
      | HasAttachmentFilterNode
      | HardAssigmentFilterNode
      | IterationCountFilterNode
      | LanguageFilterNode
      | LastMessageTypeFilterNode
      | LastReplyFilterNode
    >;
  };
}

export interface ParenthesisExpressionNode extends CstNode {
  name: 'parenthesisExpression';
  children: {
    LParen: IToken[];
    andExpression: AndExpressionNode[];
    RParen: IToken[];
  };
}
export interface AtomicExpressionNode extends CstNode {
  name: 'atomicExpression';
  children: {
    parenthesisExpression?: ParenthesisExpressionNode[];
    filter?: FilterNode[];
    UnknownKey?: IToken[];
  };
}

export interface OrExpressionNode extends CstNode {
  name: 'orExpression';
  children: {
    Left: AtomicExpressionNode[];
    Or: IToken[];
    Right?: AtomicExpressionNode[];
  };
}

export interface AndExpressionNode extends CstNode {
  name: 'andExpression';
  children: {
    Left: OrExpressionNode[];
    And: IToken[];
    Right?: OrExpressionNode[];
  };
}

export interface ExpressionNode extends CstNode {
  name: 'expression';
  children: {
    andExpression: AndExpressionNode[];
  };
}

export type Node =
  | DateAmountNode
  | DateFilterValueNode
  | OperatorNode
  | IdFilterNode
  | CreateDateFilterNode
  | AssigmentFilterValueNode
  | AssigmentFilterNode
  | HasAttachmentFilterNode
  | HardAssigmentFilterNode
  | IterationCountFilterNode
  | LanguageFilterNode
  | LastMessageTypeFilterNode
  | LastReplyFilterNode
  | FilterNode
  | ParenthesisExpressionNode
  | AtomicExpressionNode
  | OrExpressionNode
  | AndExpressionNode
  | ExpressionNode;
