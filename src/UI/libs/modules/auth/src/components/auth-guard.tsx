import React, { Fragment, PropsWithChildren, useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Login } from './login';
import { LockContainer } from '@help-line/components';
import { useAuthStore$ } from '../hooks';

export const AuthGuard = observer(({ children }: PropsWithChildren) => {
  const authStore$ = useAuthStore$();
  useEffect(() => {
    authStore$.init();
  }, [authStore$]);

  if (authStore$.loading) {
    return <LockContainer text="Login in...." />;
  }

  if (!authStore$.isAuth) {
    return (
      <LockContainer>
        <Login />
      </LockContainer>
    );
  }
  return <Fragment>{children}</Fragment>;
});
