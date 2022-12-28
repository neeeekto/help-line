import { TicketScheduleStatus } from '@help-line/entities/admin/api';
import { Tag } from 'antd';
import { ComponentProps } from 'react';

const TicketScheduleStatusToColoMap: Record<
  TicketScheduleStatus,
  ComponentProps<typeof Tag>['color']
> = {
  [TicketScheduleStatus.Planned]: 'processing',
  [TicketScheduleStatus.InQueue]: 'success',
  [TicketScheduleStatus.Problem]: 'warning',
  [TicketScheduleStatus.Dead]: 'error',
};

export interface TicketScheduleStatusLabelProps {
  status: TicketScheduleStatus;
}
export const TicketScheduleStatusLabel = (
  props: TicketScheduleStatusLabelProps
) => {
  return (
    <Tag color={TicketScheduleStatusToColoMap[props.status]}>
      {props.status}
    </Tag>
  );
};
