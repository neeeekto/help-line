import React, { useContext, useEffect } from "react";
import { Instance } from "./http.instance";
import {
  authReqInterceptor,
  errorHandlerResInterceptor,
  prefixReqInterceptor,
  projectSetterReqInterceptor,
  refetchAfter401ResInterceptor,
} from "@core/http/interceptors";
import { useAuthStore$ } from "@core/auth";
import { AuthEventContext } from "@core/auth/auth.context";
import { useSystemStore$ } from "@core/system";

export const HttpProvider: React.FC = React.memo(({ children }) => {
  const authStore = useAuthStore$();
  const systemStore = useSystemStore$();
  const authEvents = useContext(AuthEventContext);
  useEffect(() => {
    const reqInterceptors = [
      authReqInterceptor(authStore),
      prefixReqInterceptor(),
      projectSetterReqInterceptor(systemStore),
    ];
    const appliedReqInterceptors: number[] = [];
    reqInterceptors.forEach(({ onFulfilled, onRejected }) => {
      const id = Instance.interceptors.request.use(onFulfilled, onRejected);
      appliedReqInterceptors.push(id);
    });

    const resInterceptors = [
      errorHandlerResInterceptor(authStore),
      refetchAfter401ResInterceptor(authEvents),
    ];
    const appliedResInterceptors: number[] = [];
    resInterceptors.forEach(({ onFulfilled, onRejected }) => {
      const id = Instance.interceptors.response.use(onFulfilled, onRejected);
      appliedResInterceptors.push(id);
    });
    return () => {
      appliedReqInterceptors.forEach((id) => {
        Instance.interceptors.request.eject(id);
      });
      appliedResInterceptors.forEach((id) => {
        Instance.interceptors.request.eject(id);
      });
    };
  }, [authEvents, authStore, systemStore]);

  return <>{children}</>;
});
