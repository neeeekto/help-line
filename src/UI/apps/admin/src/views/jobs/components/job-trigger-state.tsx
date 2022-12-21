import React, { useEffect } from "react";
import format from "date-fns/format";
import TimeAgo from "react-timeago";
import { JobTriggerState } from "@entities/jobs";
import { Timeline, Typography } from "antd";
import { ClockCircleOutlined } from "@ant-design/icons";

export const JobTriggerStateView: React.FC<{
  state?: JobTriggerState;
  onTimeLeft?: () => void;
  className?: string;
}> = ({ state, className, onTimeLeft }) => {
  useEffect(() => {
    if (state && state.next && onTimeLeft) {
      const nextDate = new Date(state.next);
      const timerId = setTimeout(() => {
        onTimeLeft();
      }, nextDate.getTime() - Date.now());
      return () => clearTimeout(timerId);
    }
  }, [state]);
  if (!state) {
    return null;
  }
  return (
    <Timeline>
      {state.prev && (
        <Timeline.Item color="gray">
          <Typography.Text type="secondary">
            {format(new Date(state.prev), "dd.MM.yyyy HH:mm:ss")}
          </Typography.Text>
          <br />
          <Typography.Text type="secondary">
            <TimeAgo date={state.prev} />
          </Typography.Text>
        </Timeline.Item>
      )}
      {state.next && (
        <Timeline.Item dot={<ClockCircleOutlined />}>
          <div>{format(new Date(state.next), "dd.MM.yyyy HH:mm:ss")}</div>
          <div>
            <TimeAgo date={state.next} />
          </div>
        </Timeline.Item>
      )}
    </Timeline>
  );
};
