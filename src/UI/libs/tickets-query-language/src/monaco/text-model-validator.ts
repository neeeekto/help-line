import { EditorState } from './editor-state';
import { MismatchedTokenException } from 'chevrotain';
import debounce from 'lodash/debounce';
import { MarkerSeverity, editor } from 'monaco-editor';
import type * as monaco from 'monaco-editor';
type Monaco = typeof monaco;

export class TextModelValidator {
  constructor(private readonly state: EditorState) {}

  public debounceValidate = debounce(this.validate.bind(this), 500);
  public validate(monaco: Monaco, model: editor.ITextModel) {
    if (!model) return;

    const markers: editor.IMarkerData[] = [
      ...this.state.lexerLastResult.errors.map((error) => ({
        startLineNumber: error.line || 0,
        startColumn: error.column || 0,
        endLineNumber: error.line || 0,
        endColumn: (error.column || 0) + error.length,
        message: `Unexpected character ${model.getValue().at(error.offset)}`,
        severity: MarkerSeverity.Error,
      })),

      ...this.state.parsingErrors.map((error) => {
        switch (error.name) {
          case 'NotAllInputParsedException':
            return {
              startLineNumber: error.token.startLine || 0,
              startColumn: error.token.startColumn || 0,
              endLineNumber: model.getLineCount(),
              endColumn: model.getLineLength(model.getLineCount()) + 1,
              message: `Redundant input: next input is invalid and hasn't been parsed: ${model
                .getValue()
                .substring(error.token.startOffset || 0)}`,
              severity: MarkerSeverity.Warning,
            };

          case 'MismatchedTokenException':
            if (error.token.tokenType.name == 'EOF')
              return {
                startLineNumber: model.getLineCount(),
                endLineNumber: model.getLineCount(),
                startColumn:
                  ((error as MismatchedTokenException).previousToken
                    .endColumn || 0) + 1,
                endColumn: model.getLineLength(model.getLineCount()) + 1,
                message: error.message,
                severity: MarkerSeverity.Error,
              };
            return {
              startLineNumber: error.token.startLine || 0,
              startColumn: error.token.startColumn || 0,
              endLineNumber: error.token.endLine || 0,
              endColumn: (error.token.endColumn || 0) + 1,
              message: error.message,
              severity: MarkerSeverity.Error,
            };

          case 'NoViableAltException':
            if (error.token.tokenType.name == 'EOF')
              return {
                startLineNumber: model.getLineCount(),
                endLineNumber: model.getLineCount(),
                startColumn:
                  ((error as MismatchedTokenException).previousToken
                    .endColumn || 0) + 1,
                endColumn: model.getLineLength(model.getLineCount()) + 1,
                message: error.message,
                severity: MarkerSeverity.Error,
              };
            return {
              startLineNumber: error.token.startLine || 0,
              startColumn: error.token.startColumn || 0,
              endLineNumber: error.token.endLine || 0,
              endColumn: (error.token.endColumn || 0) + 1,
              message: error.message,
              severity: MarkerSeverity.Error,
            };
        }
        return;
      }),
    ].filter(Boolean) as editor.IMarkerData[];

    console.log('validate', markers);

    monaco.editor.setModelMarkers(model, 'owner', markers);
  }
}
