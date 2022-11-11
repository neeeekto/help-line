import React, { useMemo } from "react";
import format from "date-fns/format";
import ReactTimeAgo from "react-timeago";
import css from "./time-ago.module.scss";
import { Tooltip } from "antd";
import { USER_LAZY_ATTENTION_DELAY } from "@shared/constants";

export const TimeAgo: React.FC<{
  value: string | Date | number;
  useTooltip?: boolean;
}> = ({ value, useTooltip }) => {
  const dateString = useMemo(() => {
    return format(new Date(value), "dd.MM.yyyy HH:mm:ss");
  }, [value]);
  if (useTooltip) {
    return (
      <Tooltip
        title={dateString}
        className={css.box}
        mouseEnterDelay={USER_LAZY_ATTENTION_DELAY}
      >
        <ReactTimeAgo date={value} />
      </Tooltip>
    );
  }
  return (
    <span className={css.box}>
      <span className={css.relative}>
        <ReactTimeAgo date={value} />
      </span>
      <span className={css.value}>{dateString}</span>
    </span>
  );
};
