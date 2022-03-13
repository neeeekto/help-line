import { AxiosError } from 'axios';

export const handleHttpError = async <TResponse, TResult>(
  err: AxiosError<TResponse>,
  code: number,
  handler: (res: TResponse) => TResult,
) => {
  if (err.response?.status === code) {
    return handler(err.response!.data);
  }
};
