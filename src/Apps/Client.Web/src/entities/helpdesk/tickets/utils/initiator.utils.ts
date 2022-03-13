import {
  OperatorInitiator,
  UserInitiator,
  SystemInitiator,
  TicketInitiator,
} from "../types";

export function isOperatorInitiator(
  initiator: TicketInitiator
): initiator is OperatorInitiator {
  return (initiator as OperatorInitiator).$type === "OperatorInitiatorView";
}

export function isUserInitiator(
  initiator: TicketInitiator
): initiator is UserInitiator {
  return (initiator as UserInitiator).$type === "UserInitiatorView";
}

export function isSystemInitiator(
  initiator: SystemInitiator
): initiator is SystemInitiator {
  return (initiator as SystemInitiator).$type === "SystemInitiatorView";
}
