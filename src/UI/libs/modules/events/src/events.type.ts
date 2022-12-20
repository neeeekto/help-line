export type ArgumentTypes<F extends Function> = F extends (
  ...args: infer A
) => any
  ? A
  : never;

export type FunctionType = (...args: any[]) => any;
export interface FnMapper {
  [key: string]: FunctionType;
}

export type EmitFns<A extends FnMapper> = {
  [P in keyof A]: (data: ReturnType<A[P]>) => Promise<void> | void;
};

/* tslint:disable */
/* tslint:enable */
export type FnArgsToArgsType<A extends FnMapper, TResult = void> = {
  readonly [P in keyof A]: (...args: ArgumentTypes<A[P]>) => TResult;
};
