import React, { ChangeEvent, useCallback, useState } from 'react';
import { Button, Card, Input, List, Spin, Typography } from 'antd';
import { boxCss, spacingCss } from '@help-line/style-utils';
import cn from 'classnames';
import { SearchOutlined } from '@ant-design/icons';
import { useSchedulesByTicketQuery } from '@entities/helpdesk/queries';
import { TimerInfo } from '@views/tickets-timers/timer-info';
import css from './ticket-timers.module.scss';

const TicketTimersList: React.FC<{ ticketId: string }> = ({ ticketId }) => {
  const ticketTimersQuery = useSchedulesByTicketQuery(ticketId);
  if (ticketTimersQuery.isLoading) {
    return <Spin />;
  }
  if (!ticketTimersQuery.data?.length) {
    return (
      <div>
        <Typography.Text type="secondary">No timers...</Typography.Text>
      </div>
    );
  }
  return (
    <>
      {ticketTimersQuery.data?.map((x) => (
        <Card key={x.id} bordered={false} size="small" className={css.timer}>
          <TimerInfo timer={x} />
        </Card>
      ))}
    </>
  );
};

export const TicketTimers: React.FC = () => {
  const [ticketId, setTicketId] = useState('');
  const [searchId, setSearchId] = useState('');

  const onInput = useCallback(
    (evt: ChangeEvent<HTMLInputElement>) => {
      setTicketId(evt.currentTarget.value);
    },
    [setTicketId]
  );

  const onSearch = useCallback(() => {
    setSearchId(ticketId);
  }, [setSearchId, ticketId]);
  return (
    <div>
      <Card className={css.box} size="small" bordered>
        <div
          className={cn(
            boxCss.flex,
            boxCss.alignItemsCenter,
            spacingCss.marginRightSm
          )}
        >
          <Input
            prefix="Ticket ID: "
            value={ticketId}
            onChange={onInput}
            size="small"
          />
          <Button
            size="small"
            type="dashed"
            disabled={!ticketId}
            onClick={onSearch}
            className={spacingCss.marginLeftSm}
          >
            <SearchOutlined />
          </Button>
        </div>
      </Card>
      <div className={spacingCss.marginTopMd}>
        {searchId && <TicketTimersList ticketId={searchId} />}
      </div>
    </div>
  );
};
