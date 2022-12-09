import React, { Fragment, PropsWithChildren, useEffect, useState } from 'react';
import { Login } from './login';
import { LockContainer } from '@help-line/components';
import { useAuthStartup, useAuthState, useAuthUserManager } from '../store';

export const AuthGuard: React.FC<PropsWithChildren> = ({ children }) => {
  const isAuth = useAuthState();
  const userManager = useAuthUserManager();
  const loading = useAuthStartup(userManager);

  if (loading) {
    return <LockContainer text="Login in...." />;
  }

  if (!isAuth) {
    return (
      <LockContainer>
        <Login />
      </LockContainer>
    );
  }
  return <Fragment>{children}</Fragment>;
};
