import React from 'react';
import { Divider } from 'antd';
import { Initiator } from './initiator';
import { TimeAgo } from '@help-line/components';
import { TicketEvent } from '@help-line/entities/client/api';

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
