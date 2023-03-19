import {
  OperatorInitiator,
  UserInitiator,
  SystemInitiator,
  TicketInitiator,
} from '../types';

export const TicketInitiatorUtils = {
  isOperatorInitiator: (
    initiator: TicketInitiator
  ): initiator is OperatorInitiator => {
    return (initiator as OperatorInitiator).$type === 'OperatorInitiatorView';
  },

  isUserInitiator: (initiator: TicketInitiator): initiator is UserInitiator => {
    return (initiator as UserInitiator).$type === 'UserInitiatorView';
  },

  isSystemInitiator: (
    initiator: SystemInitiator
  ): initiator is SystemInitiator => {
    return (initiator as SystemInitiator).$type === 'SystemInitiatorView';
  },
};
