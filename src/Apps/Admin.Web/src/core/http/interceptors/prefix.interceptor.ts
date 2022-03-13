import { AxiosRequestConfig } from "axios";
import { environment } from "@env";
import { IReqInterceptor } from "../http.types";

export const setApiHeaders = (request: AxiosRequestConfig) => {
  request.headers = {
    ...request.headers,
  };
};

export const setApiPrefix = (request: AxiosRequestConfig) => {
  if (request.url) {
    if (
      !(request.interceptor && request.interceptor.skipPrefix) &&
      request.url.includes("/api")
    ) {
      const serverUrl = environment.serverUrl || window.location.origin;
      const url = new URL(request.url, serverUrl);
      const path = url.pathname.replace("/api", "/");
      const segments = path.split("/").filter(Boolean);
      const resultPath = [serverUrl, environment.apiPrefix, ...segments].filter(
        Boolean
      );
      request.url = `${resultPath.join("/")}/${url.search}${url.hash}`;
      setApiHeaders(request);
    }
  }
  return request;
};

export const prefixReqInterceptor = (): IReqInterceptor => ({
  onFulfilled(req) {
    return setApiPrefix(req);
  },
});
