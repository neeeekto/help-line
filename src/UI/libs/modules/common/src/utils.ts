export const callOrGetValue = <TParams extends Array<any>, TResult>(
  valOrFn: TResult | ((...params: TParams) => TResult),
  ...params: TParams
) => {
  if (typeof valOrFn === 'function') {
    return (valOrFn as Function)(...params);
  }
  return valOrFn;
};
