import React, { useCallback, useMemo, useState } from 'react';
import {
  useDeleteJobMutation,
  useFireJobMutation,
  useJobsStateQuery,
  useToggleJobMutation,
  useUpdateJobMutation,
} from '@help-line/entities/admin/query';
import {
  Button,
  Card,
  Collapse,
  Divider,
  message,
  Popconfirm,
  Switch,
  Tooltip,
} from 'antd';
import { JobTriggerStateView } from '../components/job-trigger-state';
import { spacingCss, textCss } from '@help-line/style-utils';
import cn from 'classnames';
import { DeleteOutlined, EditOutlined, FireOutlined } from '@ant-design/icons';
import css from './jobs.module.scss';
import groupBy from 'lodash/groupBy';
import cronstrue from 'cronstrue';
import { Job, JobTriggerState } from '@help-line/entities/admin/api';

const JobActions: React.FC<{ job: Job; onEdit?: () => void }> = ({
  job,
  onEdit,
}) => {
  const fireMutation = useFireJobMutation(job.id);
  const deleteMutation = useDeleteJobMutation(job.id);
  const editMutation = useUpdateJobMutation(job.id);
  const toggleMutation = useToggleJobMutation(job.id);

  const onFire = useCallback(() => {
    fireMutation.mutate();
    message.success(`Job ${job.name} fired`);
  }, [fireMutation, job]);

  const onDelete = useCallback(() => {
    deleteMutation.mutate();
  }, [fireMutation, job]);

  const onToggle = useCallback(() => {
    toggleMutation.mutate(!job.enabled);
  }, [fireMutation, job]);

  return (
    <div className={cn(css.jobActions)}>
      <Button
        type="text"
        size="small"
        disabled={
          fireMutation.isLoading ||
          editMutation.isLoading ||
          deleteMutation.isLoading
        }
        onClick={onEdit}
      >
        <EditOutlined />
      </Button>
      <Popconfirm
        placement="topRight"
        title="Do you want delete this job?"
        onConfirm={onDelete}
        okText="Yes"
        cancelText="No"
        disabled={editMutation.isLoading || job.enabled}
        okButtonProps={{ type: 'primary', danger: true }}
      >
        <Button
          type="text"
          size="small"
          loading={deleteMutation.isLoading}
          disabled={editMutation.isLoading || job.enabled}
        >
          <DeleteOutlined />
        </Button>
      </Popconfirm>

      <Switch
        size="small"
        checked={job.enabled}
        onChange={onToggle}
        loading={toggleMutation.isLoading}
        disabled={deleteMutation.isLoading}
      />
      <Tooltip title="Fire job now" mouseEnterDelay={1.5} placement="topLeft">
        <Button
          type="dashed"
          size="small"
          disabled={
            fireMutation.isLoading ||
            editMutation.isLoading ||
            deleteMutation.isLoading
          }
          onClick={onFire}
        >
          <FireOutlined />
        </Button>
      </Tooltip>
    </div>
  );
};

const JobItem: React.FC<{
  job: Job;
  onTimeLeft?: () => void;
  onEdit?: (job: Job) => void;
  status?: JobTriggerState;
}> = ({ job, onTimeLeft, status, onEdit }) => {
  const toEdit = useCallback(() => onEdit && onEdit(job), [job]);

  return (
    <Card
      key={job.id}
      title={job.name}
      hoverable
      extra={<JobActions job={job} onEdit={toEdit} />}
      className={cn(css.jobCard, { [css.jobCardDisabled]: !job.enabled })}
    >
      {job.group && (
        <div className={spacingCss.marginBottomLg}>
          <b>Group: </b>
          {job.group}
        </div>
      )}
      <Tooltip title={job.taskType} mouseEnterDelay={1.5}>
        <div className={cn(spacingCss.marginBottomLg, textCss.truncate)}>
          <b>Task: </b>
          {job.taskType}
        </div>
      </Tooltip>

      <div className={spacingCss.marginBottomLg}>
        <b>Schedule: </b>
        {cronstrue.toString(job.schedule)}
      </div>
      {status && (
        <>
          <Divider type="horizontal">State</Divider>
          <JobTriggerStateView
            className={spacingCss.marginLeftAuto}
            onTimeLeft={onTimeLeft}
            state={status}
          />
        </>
      )}
    </Card>
  );
};

export const JobsList: React.FC<{
  jobs: Job[];
  groupBy: keyof Job;
  onEdit?: (job: Job) => void;
}> = ({ jobs, groupBy: groupByField, onEdit }) => {
  const jobIds = useMemo(() => jobs.map((x) => x.id), [jobs]);
  const jobStatuses = useJobsStateQuery(jobIds);
  const onTimeLeft = useCallback(() => {
    jobStatuses.refetch();
  }, [jobStatuses, jobIds]);

  const groups = useMemo(() => {
    const groups = groupBy(jobs, groupByField);
    return Object.keys(groups).map((x) => ({ key: x, items: groups[x] }));
  }, [jobs, groupByField]);

  const groupKeys = useMemo(() => {
    return groups.map((x) => x.key);
  }, [groups, groupByField]);

  return (
    <Collapse ghost defaultActiveKey={groupKeys}>
      {groups.map((group) => (
        <Collapse.Panel header={<b>{group.key}</b>} key={group.key}>
          <div className={css.jobList}>
            {group.items
              .filter((x) => x.enabled)
              .map((job) => (
                <JobItem
                  key={job.id}
                  job={job}
                  onTimeLeft={onTimeLeft}
                  onEdit={onEdit}
                  status={(jobStatuses.data || {})[job.id]}
                />
              ))}
          </div>
          <div className={css.jobList}>
            {group.items
              .filter((x) => !x.enabled)
              .map((job) => (
                <JobItem
                  key={job.id}
                  job={job}
                  onTimeLeft={onTimeLeft}
                  onEdit={onEdit}
                  status={(jobStatuses.data || {})[job.id]}
                />
              ))}
          </div>
        </Collapse.Panel>
      ))}
    </Collapse>
  );
};
