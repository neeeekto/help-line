import css from "./aside.module.scss";
import cn from "classnames";
import React from "react";
import { useTicketContext } from "../../context.ticket";
import { boxCss } from "@shared/styles";
import { Typography } from "antd";
import { CardAssign } from "./card.assign";
import { TicketStatusTypeViewName } from "./aside.dictionary";

export const TicketStatusAside: React.FC = () => {
  const ctx = useTicketContext();

  return (
    <CardAssign
      className={cn(
        css.status,
        boxCss.flex,
        boxCss.alignItemsCenter,
        boxCss.fullWidth,
        boxCss.justifyContentSpaceBetween
      )}
    >
      <Typography.Text strong>
        {TicketStatusTypeViewName[ctx.ticket.status.type] ||
          ctx.ticket.status.type}
      </Typography.Text>
      <Typography.Text strong>{ctx.ticket.status.kind} </Typography.Text>
    </CardAssign>
  );
};
