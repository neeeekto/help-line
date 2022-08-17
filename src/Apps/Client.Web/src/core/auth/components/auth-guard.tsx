import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { useAuthStore$ } from "../auth.hooks";
import { Login } from "./login";
import { InitView } from "@core/system/components";

export const AuthGuard: React.FC = observer(({ children }) => {
  const authStore = useAuthStore$();
  useEffect(() => {
    authStore.init();
  }, [authStore]);

  if (authStore.state.loading) {
    return <InitView text={"Login in...."} />;
  }

  if (!authStore.state.isAuth) {
    return (
      <InitView>
        <Login />
      </InitView>
    );
  }
  return <>{children}</>;
});
