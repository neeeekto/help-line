import { DiRegistratorFn } from './registrator';
import { Container } from 'inversify';

export const setupDI = (...registrators: DiRegistratorFn[]) => {
  const container = new Container({ defaultScope: 'Singleton' });
  for (let i = 0; i < registrators.length; i++) {
    registrators[i](container);
  }
  return container;
};
