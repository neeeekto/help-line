import { IReqInterceptor } from "../http.types";
import { SystemStore } from "@core/system/system.store";

export const projectSetterReqInterceptor = (
  systemStore: SystemStore
): IReqInterceptor => ({
  onFulfilled(req) {
    req.headers = {
      ...req.headers,
      Project: systemStore.state.currentProject,
    };
    return req;
  },
});
