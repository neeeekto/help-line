import React from 'react';
import { FullPageContainer } from '@help-line/components';
import { TicketsTimers } from './tickets-timers';
import { useSchedulesQuery } from '@entities/helpdesk/queries';
import { Divider, Spin, Tabs, Typography } from 'antd';
import { statuses } from './ticket.utils';
import { TicketTimers } from './/ticket-timers';
import { PageSpin } from '@shared/components/page-spin';
import { boxCss, customAntdCss, utilsCss } from '@help-line/style-utils';
import cn from 'classnames';

const TicketTimersRoot: React.FC = () => {
  const schedulesQuery = useSchedulesQuery(statuses);

  return (
    <FullPageContainer>
      <Tabs type="card" className={customAntdCss.fullPageTabs}>
        <Tabs.TabPane tab="Timers" key="timers">
          {schedulesQuery.isLoading ? <PageSpin /> : <TicketsTimers />}
        </Tabs.TabPane>
        <Tabs.TabPane tab="Timers by ticket" key="timers-by-ticket">
          <TicketTimers />
        </Tabs.TabPane>
      </Tabs>
    </FullPageContainer>
  );
};

export default TicketTimersRoot;
