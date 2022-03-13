import { httpClient, HttpClient } from "@core/http";
import { User } from "@entities/user-access/users";

export const makeAuthApi = (http: HttpClient) => ({
  me: () => http.get<User>(`/api/v1/users-access/identity`, {
    interceptor: {
      disableRefreshBy401: true,
    }
  }).then((x) => x.data),
});

export const authApi = makeAuthApi(httpClient);
