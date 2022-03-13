import { stringify } from "qs";
import { environment } from "@env";
import axios from "axios";

// ====== INSTANCE ======================================
export const Instance = axios.create({
  baseURL: environment.serverUrl,
  withCredentials: false,
  paramsSerializer: (params) =>
    stringify(params, { indices: false, skipNulls: true }),
});
