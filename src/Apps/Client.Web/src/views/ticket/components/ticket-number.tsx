import React from "react";
import styles from "./styles.module.scss";
import { NumberOutlined } from "@ant-design/icons";

export const TicketNumber: React.FC<{ number: string }> = ({ number }) => {
  return (
    <div className={styles.ticketNumber}>
      <NumberOutlined />
      <span className="m-l-xs">{number}</span>
    </div>
  );
};
