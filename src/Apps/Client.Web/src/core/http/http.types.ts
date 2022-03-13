import { AxiosInstance } from "axios";
import { AxiosRequestConfig, AxiosError, AxiosResponse } from "axios";

export interface IInterceptor<Req, Err> {
  onFulfilled: (value: Req) => Req | Promise<Req>;
  onRejected?: (error: Err) => Err | Promise<Err>;
}

export type IReqInterceptor = IInterceptor<AxiosRequestConfig, Error>;
export type IResInterceptor = IInterceptor<AxiosResponse, AxiosError>;

export interface IInterceptors {
  request: IReqInterceptor[];
  response: IResInterceptor[];
}

export type HttpClient = AxiosInstance;

declare module "axios" {
  /**
   * Расширения настроект, используется в интерцепторах
   * @export
   * @interface IInterceptorAxiosRequestConfig
   * @extends {AxiosRequestConfig}
   */
  export interface AxiosRequestConfig {
    interceptor?: {
      disableAuth?: boolean;
      disableRefreshByError?: boolean;
      disableRefreshBy401?: boolean;
      skipPrefix?: boolean;
      skipErrorHandler?: boolean | number[];
    };
  }
}
