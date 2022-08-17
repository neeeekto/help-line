import { IReqInterceptor } from "../http.types";
import { AuthStore } from "@core/auth";

export const authReqInterceptor = (authStore: AuthStore): IReqInterceptor => ({
  onFulfilled(request) {
    const { interceptor = {} } = request;
    if (!interceptor.disableAuth) {
      const token = authStore.state.user?.access_token;
      const tokenType = authStore.state.user?.token_type;
      if (token) {
        request.headers["Authorization"] = `${tokenType} ${token}`;
      }
    }
    return request;
  },
});
