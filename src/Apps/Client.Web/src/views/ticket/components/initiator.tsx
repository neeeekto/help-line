import React from "react";
import { TicketInitiator } from "@entities/helpdesk/tickets";

export const Initiator: React.FC<{ who: TicketInitiator }> = ({ who }) => {
  switch (who.$type) {
    case "OperatorInitiatorView":
      return <span>Operator | {who.operatorId}</span>;
    case "SystemInitiatorView":
      return <span>System [{who.description}]</span>;
    case "UserInitiatorView":
      return <span>User [{who.userId}]</span>;
  }
};
