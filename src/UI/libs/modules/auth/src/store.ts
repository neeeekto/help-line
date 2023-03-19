import { observable, action, computed, values } from 'mobx';
import { User, UserManager } from 'oidc-client';
import { HelpLineUserProfile } from './types';
import { message } from 'antd';

export class AuthStore {
  private readonly state = observable({
    isAuth: false,
    loading: false,
    user: null as null | User,
  });

  private readonly userManager: UserManager;

  constructor(userManager: UserManager) {
    this.userManager = userManager;
  }

  get profile() {
    return this.state.user?.profile as HelpLineUserProfile;
  }

  get isAuth() {
    return this.state.isAuth;
  }

  get loading() {
    return this.state.loading;
  }

  get token() {
    if (!this.state.user) {
      return null;
    }
    return {
      type: this.state.user?.token_type,
      value: this.state.user?.access_token,
    };
  }

  readonly init = action('init', async () => {
    this.userManager.events.addUserLoaded((user: User) => {
      this.state.user = user;
      this.state.isAuth = true;
    });
    this.userManager.events.addUserUnloaded(() => {
      this.state.isAuth = false;
      this.state.user = null;
    });
    this.state.loading = true;
    try {
      const user = await this.userManager.getUser();
      if (user) {
        this.userManager.events.load(user);
      }
    } catch (e) {
      message.error({
        content: 'Auth error',
      });
    } finally {
      this.state.loading = false;
    }
  });

  login = action('logout', async () => {
    await this.userManager.signinPopup();
  });

  logout = action('logout', async () => {
    await this.userManager.signoutPopup();
  });

  clearSession = action('clearSession', async () => {
    await this.userManager.clearStaleState();
    await this.userManager.removeUser();
  });
}
