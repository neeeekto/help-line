import React, { useCallback, useMemo, useState } from 'react';
import {
  useJobsQuery,
  useToggleJobsMutation,
  useJobTasksQuery,
} from '@help-line/entities/admin/query';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { Button, Drawer, Dropdown, Menu, Radio, Spin } from 'antd';
import { FullPageContainer } from '@help-line/components';
import { JobsList } from './views/jobs.list';
import { RadioChangeEvent } from 'antd/lib/radio/interface';
import { useBoolean } from 'ahooks';
import { JobsForm } from './views/jobs.form';
import { SelectInfo } from 'rc-menu/lib/interface';
import { Job } from '@help-line/entities/admin/api';

const AddMenu: React.FC<{ onSelect?: (type: string) => void }> = ({
  onSelect,
}) => {
  const jobsTasksQuery = useJobTasksQuery();
  const tasks = useMemo(
    () => Object.keys(jobsTasksQuery.data || {}),
    [jobsTasksQuery]
  );

  const select = useCallback(
    (evt: SelectInfo) => {
      onSelect && onSelect(evt.key);
    },
    [onSelect]
  );

  return (
    <Menu onSelect={select}>
      {tasks.map((task) => (
        <Menu.Item key={task}>{task}</Menu.Item>
      ))}
    </Menu>
  );
};

export const Jobs: React.FC = () => {
  const jobsQuery = useJobsQuery();

  const [showForm, showFormActions] = useBoolean(false);
  const [showAddMenu, showAddMenuActions] = useBoolean(false);
  const [edit, setEdit] = useState<{ entity?: Job; task: string }>({
    task: '',
  });

  const [groupByField, setGroupByField] = useState<keyof Job>('group');
  const setGroupType = useCallback(
    (evt: RadioChangeEvent) => {
      setGroupByField(evt.target.value);
    },
    [setGroupByField]
  );

  const enabled = useMemo(
    () => jobsQuery.data?.filter((x) => x.enabled).map((x) => x.id) ?? [],
    [jobsQuery]
  );

  const disableAllMutation = useToggleJobsMutation(enabled);
  const onDisableAll = useCallback(
    () => disableAllMutation.mutate(false),
    [disableAllMutation]
  );

  const toEdit = useCallback(
    (job: Job) => {
      showFormActions.setTrue();
      setEdit({ entity: job, task: job.taskType });
    },
    [showFormActions, setEdit]
  );
  const toNew = useCallback(
    (task: string) => {
      showFormActions.setTrue();
      setEdit({ task: task });
      showAddMenuActions.setFalse();
    },
    [showFormActions, setEdit, showAddMenuActions]
  );

  return (
    <FullPageContainer className={cn(boxCss.flex, boxCss.flexColumn)}>
      <div
        className={cn(
          boxCss.flex,
          spacingCss.paddingSm,
          boxCss.justifyContentSpaceBetween
        )}
      >
        <div className={cn(boxCss.flex, boxCss.alignItemsCenter)}>
          <b className={spacingCss.marginRightSm}>Group by: </b>
          <Radio.Group
            value={groupByField}
            onChange={setGroupType}
            buttonStyle="solid"
          >
            <Radio.Button value="group">Group</Radio.Button>
            <Radio.Button value="taskType">Task</Radio.Button>
          </Radio.Group>
        </div>
        <div>
          {enabled.length ? (
            <Button
              type="dashed"
              danger
              onClick={onDisableAll}
              loading={disableAllMutation.isLoading}
            >
              Disable all
            </Button>
          ) : null}

          <Dropdown
            overlay={<AddMenu onSelect={toNew} />}
            placement="bottomLeft"
            open={showAddMenu}
            onOpenChange={showAddMenuActions.toggle}
            arrow
          >
            <Button className={spacingCss.marginLeftSm} type="primary">
              Add
            </Button>
          </Dropdown>
        </div>
      </div>
      <FullPageContainer
        className={cn(spacingCss.marginTopSm, boxCss.overflowAuto)}
      >
        {jobsQuery.isLoading ? (
          <Spin />
        ) : (
          <JobsList
            jobs={jobsQuery.data || []}
            groupBy={groupByField}
            onEdit={toEdit}
          />
        )}
      </FullPageContainer>
      <Drawer
        visible={showForm}
        onClose={showFormActions.setFalse}
        width={700}
        title={
          edit.entity
            ? `Edit ${edit.entity?.name} `
            : `Create job - ${edit.task}`
        }
      >
        {showForm && (
          <JobsForm
            entity={edit.entity}
            task={edit.task}
            onClose={showFormActions.setFalse}
          />
        )}
      </Drawer>
    </FullPageContainer>
  );
};
