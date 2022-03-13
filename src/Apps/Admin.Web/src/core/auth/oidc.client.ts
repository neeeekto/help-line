import { UserManager } from "oidc-client";
import { environment } from "@env";

export const makeUseManager = () => {
  return new UserManager(environment.oauth);
};
