import { AxiosError } from "axios";
import { IResInterceptor } from "../http.types";
import { AuthStore } from "@core/auth";

let lastError = 0;
const clearErrorTimeout = () => {
  setTimeout(() => {
    lastError = 0;
  }, 500);
};

export const errorHandlerResInterceptor = (
  authStore: AuthStore
): IResInterceptor => ({
  onFulfilled(req) {
    lastError = 0;
    return req;
  },
  async onRejected(err: AxiosError) {
    console.error(err);
    if (!err.response) {
      if (lastError !== -1) {
        clearErrorTimeout();
        lastError = -1;
        /*notification.error({
          message: "No answer",
          description: "Request failed :(",
          duration: 10000,
        });*/
      }
    } else {
      if (
        typeof err.config.interceptor?.skipErrorHandler === "object" &&
        err.config.interceptor?.skipErrorHandler.includes(err.response.status)
      ) {
        return Promise.reject(err);
      }
      if (lastError !== err.response.status) {
        clearErrorTimeout();
        lastError = err.response.status;
        switch (err.response.status) {
          case 401:
            /*notification.error({
              type: "error",
              message: "401",
              description: "You are not authorize",
              duration: 10000,
            });*/
            await authStore.logoutLocal();
            break;
          case 403:
            /*notification.error({
              type: "error",
              message: "403",
              description: "You have not access for this action",
              duration: 10000,
            });*/
            break;
          case 409:
            /*notification.error({
              type: "error",
              message: "409",
              description: `Conflict: ${err.response.data.message}`,
              duration: 10000,
            });*/
            break;
          case 500:
            /*notification.error({
              type: "error",
              message: "Alien attack (500)",
              // nt:disable-next-line
              description: `It looks like our servers have been hijacked by aliens and are now operating strangely, but don't worry, we've already started a rescue operation`,
              duration: 10000,
            });*/
            break;
          case 503:
            /*notification.error({
              type: "error",
              message: `Traffic jam (503)`,
              description: `Someone's slow, maybe network, maybe server, we don't know`,
              duration: 10000,
            });*/
            break;
        }
      }
    }
    return Promise.reject(err);
  },
});
