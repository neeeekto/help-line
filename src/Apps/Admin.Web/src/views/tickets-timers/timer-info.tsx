import React, { useMemo } from "react";
import { TicketSchedule } from "@entities/helpdesk";
import format from "date-fns/format";
import TimeAgo from "react-timeago";
import { Divider, Tag, Typography } from "antd";

export const TimerInfo: React.FC<{ timer: TicketSchedule }> = ({ timer }) => {
  const date = useMemo(
    () => format(new Date(timer.triggerDate), "dd.MM.yyyy HH:mm:ss"),
    [timer.triggerDate]
  );
  return (
    <div>
      <div>{date}</div>
      <Typography.Text type="secondary">
        <TimeAgo date={timer.triggerDate} />
      </Typography.Text>
    </div>
  );
};
