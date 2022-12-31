import React, { ChangeEvent, useCallback, useMemo, useState } from 'react';
import {
  Button,
  Card,
  Collapse,
  Input,
  Popconfirm,
  Result,
  Select,
  Spin,
  Table,
  Tag,
  Typography,
} from 'antd';
import groupBy from 'lodash/groupBy';
import {
  DeleteOutlined,
  RedoOutlined,
  SearchOutlined,
  SmileOutlined,
} from '@ant-design/icons';
import { TimerInfo } from './timer-info';
import css from './tickets-timers.module.scss';
import { boxCss, spacingCss } from '@help-line/style-utils';
import {
  useDeleteScheduleMutation,
  useReScheduleMutation,
  useSchedulesByTicketQuery,
  useSchedulesQuery,
} from '@help-line/entities/admin/query';
import { showStatusesPreset } from './ticket.utils';
import {
  TicketSchedule,
  TicketScheduleStatus,
} from '@help-line/entities/admin/api';
import { FullPageContainer } from '@help-line/components';
import { TicketScheduleStatusLabel } from './ticket-schedule-status-label';
import { TicketId } from '@help-line/entities/client/api';
import cn from 'classnames';
import { useDebounce } from 'ahooks';

const Actions: React.FC<{ schedule: TicketSchedule }> = ({ schedule }) => {
  const rescheduleMutation = useReScheduleMutation(schedule.id);
  const deleteMutation = useDeleteScheduleMutation(schedule.id);

  const onDelete = useCallback(() => {
    deleteMutation.mutate();
  }, [deleteMutation]);

  const onReSchedule = useCallback(() => {
    rescheduleMutation.mutate();
  }, [rescheduleMutation]);

  return (
    <>
      {schedule.status === TicketScheduleStatus.Dead && (
        <Popconfirm
          title="Are you sure?"
          onConfirm={onDelete}
          disabled={deleteMutation.isLoading}
          okButtonProps={{ danger: true }}
        >
          <Button
            size="small"
            type="text"
            loading={deleteMutation.isLoading}
            icon={<DeleteOutlined />}
          />
        </Popconfirm>
      )}
      {schedule.status === TicketScheduleStatus.Problem && (
        <Button
          size="small"
          type="text"
          onClick={onReSchedule}
          icon={<RedoOutlined />}
        ></Button>
      )}
    </>
  );
};

export const TicketsTimers: React.FC = () => {
  const [ticketIdFilter, setTicketIdFilter] = useState<TicketId>('');
  const [statusFilter, setStatusFilter] =
    useState<TicketScheduleStatus[]>(showStatusesPreset);
  const ticketIdDebounce = useDebounce<TicketId>(ticketIdFilter);
  const statusFilterDebounce =
    useDebounce<TicketScheduleStatus[]>(statusFilter);

  const schedulesQuery = useSchedulesQuery(statusFilterDebounce);
  const ticketTimersQuery = useSchedulesByTicketQuery(ticketIdDebounce);

  const onInput = useCallback(
    (evt: ChangeEvent<HTMLInputElement>) => {
      setTicketIdFilter(evt.currentTarget.value as TicketId);
    },
    [setTicketIdFilter]
  );

  const query = ticketIdDebounce ? ticketTimersQuery : schedulesQuery;

  return (
    <>
      <Typography.Title level={4}>Timers</Typography.Title>
      <div
        className={cn(boxCss.flex, boxCss.alignItemsCenter, spacingCss.spaceXs)}
      >
        <Select
          style={{ minWidth: 200 }}
          className={boxCss.flex00Auto}
          mode="multiple"
          placeholder="Status filter"
          defaultValue={statusFilter}
          onChange={(val) => setStatusFilter(val)}
        >
          {Object.values(TicketScheduleStatus).map((v) => (
            <Select.Option key={v}>{v}</Select.Option>
          ))}
        </Select>
        <Input
          prefix="Ticket ID: "
          placeholder="X-XXXXXXX"
          value={ticketIdFilter}
          onChange={onInput}
          allowClear
        />
      </div>
      <Table
        className={spacingCss.marginTopSm}
        dataSource={query.data}
        size={'small'}
        pagination={false}
        loading={query.isLoading}
      >
        <Table.Column dataIndex={'ticketId'} title={'Ticket'}></Table.Column>
        <Table.Column
          dataIndex={'status'}
          title={'Status'}
          render={(status) => <TicketScheduleStatusLabel status={status} />}
        ></Table.Column>
        <Table.Column
          title={'Timer'}
          render={(_, item) => <TimerInfo timer={item as TicketSchedule} />}
        ></Table.Column>
        <Table.Column
          dataIndex={'details'}
          title={'Details'}
          width={'50%'}
        ></Table.Column>

        <Table.Column
          dataIndex={''}
          title={'Actions'}
          render={(v, item) => <Actions schedule={item as TicketSchedule} />}
        ></Table.Column>
      </Table>
    </>
  );
};
