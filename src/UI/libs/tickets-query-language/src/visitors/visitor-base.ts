import { CstNode, ICstVisitor } from 'chevrotain';
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
  Node,
  OperatorNode,
  OrExpressionNode,
  ParenthesisExpressionNode,
} from '../parser';

const first = <T>(arr?: Array<T>) => arr?.[0] ?? undefined;

export abstract class VisitorBase {
  visit(cstNode: CstNode | CstNode[]) {
    const node = (Array.isArray(cstNode) ? first(cstNode) : cstNode) as Node;
    if (!node) {
      return null;
    }

    const methodName = node.name.charAt(0).toUpperCase() + node.name.slice(1);

    return (this as any)[`visit${methodName}`](node);
  }

  protected abstract visitDateFilterValue(node: DateFilterValueNode): any;
  protected abstract visitOperator(node: OperatorNode): any;
  protected abstract visitIdFilter(node: IdFilterNode): any;
  protected abstract visitCreateDateFilter(node: CreateDateFilterNode): any;
  protected abstract visitAssigmentFilterValue(
    node: AssigmentFilterValueNode
  ): any;
  protected abstract visitAssigmentFilter(node: AssigmentFilterNode): any;
  protected abstract visitHasAttachmentFilter(
    node: HasAttachmentFilterNode
  ): any;
  protected abstract visitHardAssigmentFilter(
    node: HardAssigmentFilterNode
  ): any;
  protected abstract visitIterationCountFilter(
    node: IterationCountFilterNode
  ): any;
  protected abstract visitLanguageFilter(node: LanguageFilterNode): any;
  protected abstract visitLastMessageTypeFilter(
    node: LastMessageTypeFilterNode
  ): any;
  protected abstract visitLastReplyFilter(node: LastReplyFilterNode): any;
  protected abstract visitFilter(node: FilterNode): any;
  protected abstract visitParenthesisExpression(
    node: ParenthesisExpressionNode
  ): any;
  protected abstract visitAtomicExpression(node: AtomicExpressionNode): any;
  protected abstract visitOrExpression(node: OrExpressionNode): any;
  protected abstract visitAndExpression(node: AndExpressionNode): any;
  protected abstract visitExpression(node: ExpressionNode): any;
}
