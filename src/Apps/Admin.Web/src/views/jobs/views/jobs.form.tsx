import React, { useCallback, useMemo } from "react";
import {
  Job,
  JobData,
  useCreateJobMutation,
  useJobsQuery,
  useJobTasksQuery,
  useUpdateJobMutation,
} from "@entities/jobs";
import { AutoComplete, Button, Form, Input, Typography } from "antd";
import { ReQuartzCron } from "@sbzen/re-cron";
import css from "./jobs.module.scss";
import cronstrue from "cronstrue";
import { DataBuilder } from "@shared/components/data-builder";
import { boxCss, spacingCss } from "@shared/styles";
import cn from "classnames";
import groupBy from "lodash/groupBy";
import ReactJson, { ReactJsonViewProps } from "react-json-view";

const Json: React.FC<Partial<ReactJsonViewProps>> = (props: any) => (
  <ReactJson {...props} />
);

export const JobsForm: React.FC<{
  entity?: Job;
  task: string;
  onClose?: () => void;
}> = ({ entity, onClose, task }) => {
  const jobsQuery = useJobsQuery();
  const jobsTasksQuery = useJobTasksQuery();
  const createJobMutation = useCreateJobMutation(task);
  const updateJobMutation = useUpdateJobMutation(entity?.id || "");

  const groupOptions = useMemo(
    () => Object.keys(groupBy(jobsQuery.data || [], "group")),
    [jobsQuery]
  );
  const dataDescription = useMemo(
    () => (jobsTasksQuery.data || {})[task],
    [task, jobsTasksQuery]
  );
  const onSave = useCallback(
    async (value: JobData) => {
      try {
        if (entity) {
          await updateJobMutation.mutateAsync(value);
        } else {
          await createJobMutation.mutateAsync(value);
        }
        onClose && onClose();
      } finally {
      }
    },
    [entity, createJobMutation, updateJobMutation]
  );
  return (
    <Form
      name="job"
      initialValues={
        entity || {
          taskType: task,
          schedule: "* * * ? * *",
          data: dataDescription ? { $type: dataDescription.root } : null,
        }
      }
      autoComplete="off"
      layout="vertical"
      onFinish={onSave}
    >
      <Form.Item
        label="Name"
        name="name"
        rules={[{ required: true, message: "Please input job name!" }]}
      >
        <Input />
      </Form.Item>

      <Form.Item label="Group" name="group" style={{ marginBottom: 0 }}>
        <Input />
      </Form.Item>
      <Form.Item shouldUpdate>
        {(form) =>
          groupOptions.map((x) => (
            <Button
              size="small"
              key={x}
              type="dashed"
              onClick={() => form.setFieldsValue({ group: x })}
            >
              {x}
            </Button>
          ))
        }
      </Form.Item>

      <Form.Item label="Schedule" name="schedule">
        <Input readOnly />
      </Form.Item>

      <Form.Item className={css.jobFormCronEl} shouldUpdate>
        {(form) => (
          <>
            <Typography.Text type="secondary">
              {cronstrue.toString(form.getFieldValue("schedule"))}
            </Typography.Text>
            <div className={spacingCss.paddingBottomSm} />
            <ReQuartzCron
              value={form.getFieldValue("schedule")}
              onChange={(value) => {
                form.setFieldsValue({ schedule: value });
              }}
            />
          </>
        )}
      </Form.Item>

      {dataDescription && (
        <>
          <div className={spacingCss.marginBottomSm}>Data</div>
          <Form.Item shouldUpdate>
            {(form) => (
              <DataBuilder
                className={css.jobFormData}
                description={dataDescription}
                value={form.getFieldValue("data")}
                onChange={(val) => form.setFieldsValue({ data: val })}
              />
            )}
          </Form.Item>
          <Form.Item
            label="Data JSON"
            name="data"
            rules={[{ required: true, message: "Please add data!" }]}
            style={{ margin: 0 }}
            valuePropName="src"
          >
            <Json displayDataTypes={false} />
          </Form.Item>
        </>
      )}

      <div className={cn(boxCss.flex, boxCss.justifyContentEnd)}>
        <Button htmlType="button" onClick={onClose}>
          Cancel
        </Button>
        <Button
          type="primary"
          htmlType="submit"
          className={spacingCss.marginLeftSm}
          loading={createJobMutation.isLoading || updateJobMutation.isLoading}
        >
          Save
        </Button>
      </div>
    </Form>
  );
};
