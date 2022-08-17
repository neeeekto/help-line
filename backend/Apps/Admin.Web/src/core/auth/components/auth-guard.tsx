import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { useAuthStore$ } from "../auth.context";
import { Login } from "./login";
import { FullPageContainer } from "@shared/components/full-page-container";
import { Spin } from "antd";
import { PageSpin } from "@shared/components/page-spin";
import css from "./auth.module.scss";

export const AuthGuard: React.FC = observer(({ children }) => {
  const authStore = useAuthStore$();
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    setLoading(true);
    authStore.init().finally(() => {
      setLoading(false);
    });
  }, []);

  if (loading) {
    return (
      <FullPageContainer useCenterPlacement className={css.bg}>
        <Spin tip={"Login in...."} />
      </FullPageContainer>
    );
  }

  if (!authStore.state.isAuth) {
    return (
      <FullPageContainer useCenterPlacement className={css.bg}>
        <Login />
      </FullPageContainer>
    );
  }
  return <>{children}</>;
});
