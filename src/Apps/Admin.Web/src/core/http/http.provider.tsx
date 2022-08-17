import React, { useContext, useEffect, useMemo } from "react";
import { Instance } from "./http.instance";
import {
  authReqInterceptor,
  errorHandlerResInterceptor,
  prefixReqInterceptor,
  refetchAfter401ResInterceptor,
} from "@core/http/interceptors";
import { useAuthStore$ } from "@core/auth";
import { AuthEventContext } from "@core/auth/auth.context";

export const HttpProvider: React.FC = React.memo(({ children }) => {
  const authStore = useAuthStore$();
  const authEvents = useContext(AuthEventContext);
  useEffect(() => {
    const reqInterceptors = [
      authReqInterceptor(authStore),
      prefixReqInterceptor(),
    ];
    reqInterceptors.forEach(({ onFulfilled, onRejected }) => {
      Instance.interceptors.request.use(onFulfilled, onRejected);
    });

    const resInterceptors = [
      errorHandlerResInterceptor(authStore),
      refetchAfter401ResInterceptor(authEvents),
    ];
    resInterceptors.forEach(({ onFulfilled, onRejected }) => {
      Instance.interceptors.response.use(onFulfilled, onRejected);
    });
  }, []);

  return <>{children}</>;
});
