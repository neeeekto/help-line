import React, { useMemo } from "react";
import format from "date-fns/format";
import ReactTimeAgo from "react-timeago";
import css from "./time-ago.module.scss";

export const TimeAgo: React.FC<{ value: string | Date | number }> = ({
  value,
}) => {
  const dateString = useMemo(() => {
    return format(new Date(value), "dd-MM-yyyy HH:mm:ss");
  }, [value]);
  return (
    <span className={css.box}>
      <span className={css.relative}>
        <ReactTimeAgo date={value} />
      </span>
      <span className={css.value}>{dateString}</span>
    </span>
  );
};
