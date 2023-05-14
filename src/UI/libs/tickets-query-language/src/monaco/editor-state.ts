import {
  CstNode,
  ILexingError,
  ILexingResult,
  IRecognitionException,
} from 'chevrotain';
import { TicketsQueryLexer } from '../lexer';
import { TicketsQueryParser } from '../parser';
import { editor } from 'monaco-editor';
import ITextModel = editor.ITextModel;
import IStandaloneCodeEditor = editor.IStandaloneCodeEditor;

export interface TokenPosition {
  startColumn: number;
  endColumn: number;
  startLine: number;
  endLine: number;
}

export class EditorState {
  semanticErrors: TokenPosition[][] = [];
  lexerLastResult!: ILexingResult;
  cst!: CstNode;

  constructor(
    private readonly lexer: TicketsQueryLexer,
    private readonly parser: TicketsQueryParser
  ) {}

  update(model: ITextModel) {
    this.updateState(model.getValue());
  }
  init(model: ITextModel) {
    const value = model.getValue();
    this.updateState(value);
  }

  private updateState(value: string) {
    this.lexerLastResult = this.lexer.tokenize(value);
    this.parser.input = this.lexerLastResult.tokens;
    this.cst = this.parser.$expression();
  }

  get parsingErrors() {
    return this.parser.errors;
  }
}
