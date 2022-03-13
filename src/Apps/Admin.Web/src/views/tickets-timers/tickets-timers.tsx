import React, { useCallback, useMemo } from "react";
import {
  Button,
  Card,
  Collapse,
  Divider,
  Popconfirm,
  Result,
  Tag,
  Typography,
} from "antd";
import groupBy from "lodash/groupBy";
import { DeleteOutlined, RedoOutlined, SmileOutlined } from "@ant-design/icons";
import { TimerInfo } from "./timer-info";
import css from "./tickets-timers.module.scss";
import { boxCss, spacingCss } from "@shared/styles";
import {
  useDeleteScheduleMutation,
  useReScheduleMutation,
  useSchedulesQuery,
} from "@entities/helpdesk/queries";
import { statuses } from "./ticket.utils";
import { TicketSchedule, TicketScheduleStatus } from "@entities/helpdesk";

const CardActions: React.FC<{ schedule: TicketSchedule }> = ({ schedule }) => {
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
          <Button size="small" type="text" loading={deleteMutation.isLoading}>
            <DeleteOutlined />
          </Button>
        </Popconfirm>
      )}
      {schedule.status === TicketScheduleStatus.Problem && (
        <Button size="small" type="text" onClick={onReSchedule}>
          <RedoOutlined />
        </Button>
      )}
    </>
  );
};

const CardTitle: React.FC<{ schedule: TicketSchedule }> = ({ schedule }) => (
  <div className={boxCss.flex}>
    <b className={spacingCss.marginRightLg}>{schedule.ticketId}</b>
    <Tag
      color={
        schedule.status === TicketScheduleStatus.Problem ? "error" : "warning"
      }
    >
      {schedule.status}
    </Tag>
  </div>
);

export const TicketsTimers: React.FC = () => {
  const schedulesQuery = useSchedulesQuery(statuses);

  const groups = useMemo(() => {
    const groups = groupBy(schedulesQuery.data || [], "status");
    return Object.keys(groups).map((x) => ({ name: x, items: groups[x] }));
  }, [schedulesQuery]);
  return (
    <Collapse ghost defaultActiveKey={groups.map((x) => x.name)}>
      {groups.map((x) => (
        <Collapse.Panel header={x.name} key={x.name}>
          {x.items.map((t) => (
            <Card
              key={t.id}
              size="small"
              title={<CardTitle schedule={t} />}
              className={css.timerCard}
              bordered
              extra={<CardActions schedule={t} />}
            >
              <TimerInfo timer={t} />
              {t.details && (
                <div className={spacingCss.marginTopSm}>
                  <Typography.Text type="danger">{t.details}</Typography.Text>
                </div>
              )}
            </Card>
          ))}
        </Collapse.Panel>
      ))}
      {groups.length === 0 && (
        <div>
          <Result
            icon={<SmileOutlined />}
            title="Great, there is no failed tickets timers!"
            extra={
              <Button type="primary" onClick={() => schedulesQuery.refetch()}>
                Reload
              </Button>
            }
          />
        </div>
      )}
    </Collapse>
  );
};
