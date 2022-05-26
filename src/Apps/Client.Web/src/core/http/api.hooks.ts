import { HttpClient } from "@core/http/http.types";
import { useContext } from "react";
import { ApiRegistryContext } from "./api.context";
import { httpClient } from "@core/http/index";

export const makeOrGetApiInstance = <T = any>(
  registry: Map<Function, any>,
  factory: (http: HttpClient) => T
): T => {
  let current = registry.get(factory);
  if (!current) {
    current = factory(httpClient);
    registry.set(factory, current);
  }
  return current as T;
};

export const useApiInstance = <T = any>(
  factory: (http: HttpClient) => T
): T => {
  const registry = useContext(ApiRegistryContext);
  return makeOrGetApiInstance(registry, factory);
};

export const makeUseHookForApi =
  <T>(factory: (http: HttpClient) => T) =>
  () =>
    useApiInstance(factory);
