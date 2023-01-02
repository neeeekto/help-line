import React from 'react';
import {
  TicketInitiator,
  TicketInitiatorType,
} from '@help-line/entities/client/api';

export const Initiator: React.FC<{ who: TicketInitiator }> = ({ who }) => {
  switch (who.$type) {
    case TicketInitiatorType.OperatorInitiatorView:
      return <span>Operator | {who.operatorId}</span>;
    case TicketInitiatorType.SystemInitiatorView:
      return <span>System [{who.description}]</span>;
    case TicketInitiatorType.UserInitiatorView:
      return <span>User [{who.userId}]</span>;
    default:
      return <></>;
  }
};
