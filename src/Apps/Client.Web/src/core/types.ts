export type ArgumentTypes<F extends Function> = F extends (
  ...args: infer A
) => any
  ? A
  : never;

export type FunctionType<TArgs = any, TResult = any> = (
    ...args: TArgs[]
) => TResult;
