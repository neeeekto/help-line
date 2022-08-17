import React, {
  PropsWithChildren,
  ReactElement,
  ReactNode,
  useContext,
  useEffect,
  useMemo,
} from "react";
import { ApiRegistryContext } from "@core/http/api.context";
import { FunctionType } from "@core/types";
import { makeOrGetApiInstance, useApiInstance } from "@core/http/api.hooks";
import { HttpClient } from "@core/http";
import { QueryClient, QueryClientProvider, useQueryClient } from "react-query";
import { ReactQueryDevtools } from "react-query/devtools";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: false,
      cacheTime: Number.POSITIVE_INFINITY,
    },
  },
});

interface ApiStubParams<TApiFactory extends FunctionType<HttpClient>> {
  apiFactory: TApiFactory;
  stub: Partial<ReturnType<TApiFactory>>;
}

export function Stub<TApiFactory extends FunctionType<HttpClient>>(
  params: ApiStubParams<TApiFactory>
) {
  return null;
}

export function ApiStubsRoot<
  TApiFactory extends FunctionType<HttpClient>
>(params: {
  children:
    | ReactNode
    | ReactElement<ApiStubParams<TApiFactory>>
    | Array<ReactElement<ApiStubParams<TApiFactory>>>;
}) {
  const childrenArr = Array.isArray(params.children)
    ? params.children
    : [params.children];
  const stubChildren = (
    childrenArr as Array<ReactElement<ApiStubParams<TApiFactory>>>
  ).filter((x) => x.type == Stub);

  const currentRegistry = useContext(ApiRegistryContext);

  const newRegistry = useMemo(() => {
    const copyOfRegistry = new Map(currentRegistry);
    for (let stubChild of stubChildren) {
      const currentInstance = makeOrGetApiInstance(
        currentRegistry,
        stubChild.props.apiFactory
      );
      copyOfRegistry.set(stubChild.props.apiFactory, {
        ...currentInstance,
        ...stubChild.props.stub,
      });
    }

    return copyOfRegistry;
  }, [currentRegistry]);
  const client = useMemo(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            retry: false,
            cacheTime: Number.POSITIVE_INFINITY,
            staleTime: Number.POSITIVE_INFINITY,
          },
        },
      }),
    [currentRegistry]
  );
  return (
    <QueryClientProvider client={client}>
      <ApiRegistryContext.Provider value={newRegistry}>
        {params.children}
      </ApiRegistryContext.Provider>
      <ReactQueryDevtools position="bottom-right" />
    </QueryClientProvider>
  );
}
ApiStubsRoot.Stub = Stub;
