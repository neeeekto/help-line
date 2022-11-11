import React, { Fragment, PropsWithChildren, useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { useAuthStore$ } from '../auth.hooks';
import css from './auth-guard.module.scss';
import { Spin } from 'antd';
import cn from 'classnames';
import { Login } from './login';

const Center: React.FC<PropsWithChildren<{ className?: string }>> = ({
  children,
  className,
}) => <div className={cn(css['root'], className)}>{children}</div>;

export const AuthGuard: React.FC<
  PropsWithChildren<{
    className?: string;
  }>
> = observer(({ children, className }) => {
  const authStore = useAuthStore$();
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    setLoading(true);
    authStore.init().finally(() => {
      setLoading(false);
    });
  }, [authStore]);

  if (loading) {
    return (
      <Center className={className}>
        <Spin></Spin>
      </Center>
    );
  }

  if (!authStore.state.isAuth) {
    return (
      <Center className={className}>
        <Login />
      </Center>
    );
  }
  return <Fragment>{children}</Fragment>;
});
