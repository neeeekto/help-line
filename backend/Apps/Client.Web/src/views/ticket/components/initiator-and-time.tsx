import React from "react";
import { TicketEvent } from "@entities/helpdesk/tickets";
import { TimeAgo } from "@shared/components/time-ago";
import { Divider } from "antd";
import { Initiator } from "./initiator";

export const InitiatorAndTime: React.FC<{
  evt: TicketEvent;
  className?: string;
}> = ({ evt, className }) => {
  return (
    <span className={className}>
      <TimeAgo value={evt.createDate} />
      <Divider type="vertical" />
      <Initiator who={evt.initiator} />
    </span>
  );
};
