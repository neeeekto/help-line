import parse from 'date-fns/parse';

import {
  TicketAssigmentFilter,
  TicketAssigmentFilterValue,
  TicketAttachmentFilter,
  TicketCreateDateFilter,
  TicketDiscussionStateMessageType,
  TicketFilterDateValue,
  TicketFilterDateValueAction,
  TicketFilterDateValueActionOperation,
  TicketFilterGroup,
  TicketFilterOperators,
  TicketHardAssigmentFilter,
  TicketIdFilter,
  TicketIterationCountFilter,
  TicketLanguageFilter,
  TicketLastMessageTypeFilter,
  TicketLastReplyFilter,
  TicketNoopFilter,
} from '@help-line/entities/client/api';
import {
  AndExpressionNode,
  AssigmentFilterNode,
  AssigmentFilterValueNode,
  AtomicExpressionNode,
  CreateDateFilterNode,
  DateFilterValueNode,
  ExpressionNode,
  FilterNode,
  HardAssigmentFilterNode,
  HasAttachmentFilterNode,
  IdFilterNode,
  IterationCountFilterNode,
  LanguageFilterNode,
  LastMessageTypeFilterNode,
  LastReplyFilterNode,
  OperatorNode,
  OrExpressionNode,
  ParenthesisExpressionNode,
} from '../parser';
import { VisitorBase } from './visitor-base';
import { IToken } from 'chevrotain';

const first = <T>(arr?: Array<T>) => arr?.[0] ?? undefined;

export class TicketFilterBuilderVisitor extends VisitorBase {
  protected visitAndExpression({ children }: AndExpressionNode) {
    const filters = [children.Left, children.Right || []]
      .flat()
      .map((node) => this.visit(node));

    if (filters.length === 1) {
      return first(filters);
    }
    return {
      $type: 'TicketFilterGroup',
      intersection: true,
      filters,
    } as TicketFilterGroup;
  }

  protected visitOrExpression({ children }: OrExpressionNode) {
    const filters = [children.Left, children.Right || []]
      .flat()
      .map((node) => this.visit(node));

    if (filters.length === 1) {
      return first(filters);
    }

    return {
      $type: 'TicketFilterGroup',
      intersection: false,
      filters: [children.Left, children.Right || []]
        .flat()
        .map((node) => this.visit(node)),
    } as TicketFilterGroup;
  }

  protected visitAssigmentFilter({ children }: AssigmentFilterNode) {
    return {
      $type: 'TicketAssigmentFilter',
      values: children.assigmentFilterValue.map((node) =>
        this.visitAssigmentFilterValue(node)
      ),
    } as TicketAssigmentFilter;
  }

  protected visitAssigmentFilterValue({ children }: AssigmentFilterValueNode) {
    if (children.AssigmentMe?.length) {
      return {
        $type: 'CurrentOperator',
      } as TicketAssigmentFilterValue;
    }
    if (children.AssigmentNone?.length) {
      return {
        $type: 'Unassigned',
      } as TicketAssigmentFilterValue;
    }
    return {
      $type: 'Operator',
      id: first(children.StringValue)?.image || '',
    } as TicketAssigmentFilterValue;
  }

  protected visitAtomicExpression({ children }: AtomicExpressionNode) {
    if (children.parenthesisExpression) {
      return this.visit(children.parenthesisExpression || children.filter);
    }
    if (children.filter) {
      return this.visit(children.filter);
    }

    return this.noopFilter;
  }

  protected visitCreateDateFilter({ children }: CreateDateFilterNode) {
    return {
      $type: 'TicketCreateDateFilter',
      value: this.visit(children.dateFilterValue),
    } as TicketCreateDateFilter;
  }

  protected visitDateFilterValue(node: DateFilterValueNode) {
    const result = {
      $type: 'FilterDateValue',
      operator: this.visit(node.children.operator),
      dateTime: this.extractDateValue(node),
    } as TicketFilterDateValue;
    const action = this.extractDateAction(node);
    if (action) {
      result.action = action;
    }
    return result;
  }

  protected visitExpression(node: ExpressionNode) {
    return this.visit(node.children.andExpression);
  }

  protected visitFilter({ children }: FilterNode) {
    return this.visit(children.Value);
  }

  protected visitHardAssigmentFilter(node: HardAssigmentFilterNode) {
    return {
      $type: 'TicketHardAssigmentFilter',
      value: this.extractBoolValue(node.children.BooleanValue),
    } as TicketHardAssigmentFilter;
  }

  protected visitHasAttachmentFilter(node: HasAttachmentFilterNode) {
    return {
      $type: 'TicketAttachmentFilter',
      value: this.extractBoolValue(node.children.BooleanValue),
    } as TicketAttachmentFilter;
  }

  protected visitIdFilter(node: IdFilterNode) {
    return {
      $type: 'TicketIdFilter',
      value: first(node.children.TicketNumber)!.image,
    } as TicketIdFilter;
  }

  protected visitIterationCountFilter(node: IterationCountFilterNode) {
    return {
      $type: 'TicketIterationCountFilter',
      operator: this.visit(node.children.operator),
      value: this.extractIntegerValue(node.children.Integer),
    } as TicketIterationCountFilter;
  }

  protected visitLanguageFilter(node: LanguageFilterNode) {
    return {
      $type: 'TicketLanguageFilter',
      value: node.children.StringValue.map((strNode) => strNode.image),
    } as TicketLanguageFilter;
  }

  protected visitLastMessageTypeFilter(node: LastMessageTypeFilterNode) {
    return {
      $type: 'TicketLastMessageTypeFilter',
      value:
        first(node.children.StringValue)!.image === 'incoming'
          ? TicketDiscussionStateMessageType.Incoming
          : TicketDiscussionStateMessageType.Outgoin,
    } as TicketLastMessageTypeFilter;
  }

  protected visitLastReplyFilter(node: LastReplyFilterNode) {
    return {
      $type: 'TicketLastReplyFilter',
      value: this.visit(node.children.dateFilterValue),
    } as TicketLastReplyFilter;
  }

  protected visitOperator({ children }: OperatorNode) {
    if (children.NotEqual) {
      return TicketFilterOperators.NotEqual;
    }
    if (children.Less) {
      return TicketFilterOperators.Less;
    }
    if (children.LessOrEqual) {
      return TicketFilterOperators.LessOrEqual;
    }
    if (children.GreatOrEqual) {
      return TicketFilterOperators.GreatOrEqual;
    }
    if (children.Great) {
      return TicketFilterOperators.Great;
    }

    return TicketFilterOperators.Equal;
  }

  protected visitParenthesisExpression(node: ParenthesisExpressionNode) {
    return this.visit(node.children.andExpression);
  }

  private noopFilter() {
    return {
      $type: 'TicketNoopFilter',
    } as TicketNoopFilter;
  }

  private extractDateValue({ children }: DateFilterValueNode) {
    if (children.DateNow) {
      return null;
    }
    const firstDate = first(children.dateValue);
    if (firstDate) {
      return parse(firstDate.image, 'dd.MM.YYYY HH:mm:ss', new Date());
    }

    return undefined;
  }

  protected extractBoolValue(nodes: IToken[]) {
    return first(nodes)?.image === 'true';
  }

  protected extractIntegerValue(nodes: IToken[]) {
    return Number.parseInt(first(nodes)?.image || '');
  }

  private extractDateAction({ children }: DateFilterValueNode) {
    if (children.Plus || children.Minus) {
      return {
        operation: children.Plus
          ? TicketFilterDateValueActionOperation.Add
          : TicketFilterDateValueActionOperation.Sub,
        amount: this.mapDateDurationNodesToDateSpan(
          children.DateDuration || []
        ),
      } as TicketFilterDateValueAction;
    }

    return undefined;
  }

  private mapDateDurationNodesToDateSpan(nodes: IToken[]) {
    debugger;
    const tokens = nodes.reduce((res, node) => {
      const type = node.image.slice(-1);
      const value = node.image.slice(0, -1);
      (res as any)[type] = Number.parseInt(value);
      return res;
    }, {} as Record<'h' | 'm' | 's' | 'd', number>);

    const time = [tokens.h, tokens.m, tokens.s]
      .map((amount) => amount || 0)
      .join(':');

    return `${tokens.d || 0}.${time}`;
  }
}
