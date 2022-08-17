import { IReqInterceptor, IResInterceptor } from "../http.types";
import { Instance } from "../http.instance";
import { AuthEvents } from "@core/auth";

/*
const getErrorForRequest = (error: AxiosError, req: AxiosRequestConfig) => {
  if (req === error.config) {
    return error;
  }
  const newError = { ...error, config: req };
  return newError;
};*/

export const refetchAfter401ResInterceptor = (
  authEvents: AuthEvents
): IResInterceptor => {
  let refreshSubscribers: Array<{
    success: () => void;
    error: () => void;
  }> = [];

  const addSubscribers = (success: () => void, error: () => void) => {
    refreshSubscribers.push({ success, error });
  };
  const onRefreshed = () => {
    refreshSubscribers.forEach((cb) => cb.success());
    refreshSubscribers = [];
  };

  const onError = () => {
    refreshSubscribers.forEach((cb) => cb.error());
    refreshSubscribers = [];
  };
  authEvents.on((status) => {
    if (status) {
      onRefreshed();
    } else {
      onError();
    }
  });

  return {
    onFulfilled(req) {
      return req;
    },
    onRejected(error) {
      if (
        error.response &&
        error.response.status === 401 &&
        !error.response.config.interceptor?.disableRefreshBy401
      ) {
        const origRequest = error.config;
        return new Promise<any>((resolve, reject) => {
          addSubscribers(
            () =>
              resolve(
                Instance.request({
                  ...origRequest,
                  interceptor: {
                    ...origRequest.interceptor,
                    skipPrefix: true,
                  },
                })
              ),
            () => reject(error)
          );
        });
      }
      return Promise.reject(error);
    },
  };
};

export const refetchAfter401ReqInterceptor = (): IReqInterceptor => ({
  onFulfilled(req) {
    return req;
  },
});
