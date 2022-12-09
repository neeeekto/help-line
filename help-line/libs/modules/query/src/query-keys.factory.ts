import { QueryKey } from '@tanstack/react-query';

export interface CreateQueryCtx {
  makeKey: (...keys: QueryKey) => QueryKey;
  root: QueryKey;
}
export const createQueryKeys = <T>(
  root: QueryKey,
  factory: (ctx: CreateQueryCtx) => T
): T & { root: QueryKey } => {
  const ctx: CreateQueryCtx = {
    root,
    makeKey: (...keys: QueryKey) => [...root, ...keys],
  };
  const factoryResult = factory(ctx);

  return {
    ...factoryResult,
    root,
  };
};
