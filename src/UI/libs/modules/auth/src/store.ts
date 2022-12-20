import { useBoolean } from 'ahooks';
import { useContext, useEffect, useMemo } from 'react';
import { User, UserManager, UserManagerSettings } from 'oidc-client';
import { message } from 'antd';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { HelpLineUserProfile } from './types';
import { AuthUserManagerContext } from './context';

const authKeys = {
  root: ['#auth'],
  state: () => [...authKeys.root, 'state'],
  user: () => [...authKeys.root, 'user'],
};

export const useAuthUserManager = () => useContext(AuthUserManagerContext);

export const useAuthState = () => {
  const query = useQuery(authKeys.state(), () => false, {
    staleTime: Infinity,
  });
  return query.data!;
};

export const useAuthUser = () => {
  const query = useQuery<null | User>(authKeys.user(), () => null, {
    staleTime: Infinity,
  });
  return query.data as null | User;
};

export const useAuthProfile = () => {
  const query = useAuthUser();
  return query?.profile as HelpLineUserProfile | undefined;
};

export const useLogoutAction = () => {
  const userManager = useAuthUserManager();

  return useMutation([...authKeys.root, 'logout'], () =>
    userManager.signoutPopup()
  );
};

export const useLogoutByNetworkAction = () => {
  const userManager = useAuthUserManager();

  return useMutation([...authKeys.root, 'logoutByNetwork'], async () => {
    await userManager.clearStaleState();
    await userManager.removeUser();
  });
};

export const useLoginAction = () => {
  const userManager = useAuthUserManager();

  return useMutation([...authKeys.root, 'login'], () =>
    userManager.signinPopup()
  );
};

export const useAuthStartup = (userManager: UserManager) => {
  const [loading, loadingActions] = useBoolean(true);
  const client = useQueryClient();
  useEffect(() => {
    const userLoaded = (user: User) => {
      client.setQueryData(authKeys.user(), user);
      client.setQueryData(authKeys.state(), true);
    };

    const userUnloaded = () => {
      client.setQueryData(authKeys.user(), null);
      client.setQueryData(authKeys.state(), false);
    };

    userManager.events.addUserLoaded(userLoaded);

    userManager.events.addUserUnloaded(userUnloaded);

    loadingActions.setTrue();
    userManager
      .getUser()
      .then((user) => {
        if (user) {
          userManager.events.load(user);
        }
        loadingActions.setFalse();
      })
      .catch(() => {
        message.error({
          content: 'Auth error',
        });
      });
    return () => {
      userManager?.events?.removeUserLoaded(userLoaded);
      userManager?.events?.removeUserLoaded(userUnloaded);
    };
  }, [userManager]);

  return loading;
};
