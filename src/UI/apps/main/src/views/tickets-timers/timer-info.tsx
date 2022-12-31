import React, { useMemo } from 'react';
import { TicketSchedule } from '@help-line/entities/admin/api';
import format from 'date-fns/format';
import { TimeAgo } from '@help-line/components';

export const TimerInfo: React.FC<{ timer: TicketSchedule }> = ({ timer }) => {
  const date = useMemo(
    () => format(new Date(timer.triggerDate), 'dd.MM.yyyy HH:mm:ss'),
    [timer.triggerDate]
  );
  return (
    <div>
      {date} {'->'} <TimeAgo value={timer.triggerDate} />
    </div>
  );
};
