import React, { useCallback, useState } from 'react';
import cn from 'classnames';
import {
  Button,
  Form,
  Input,
  message,
  Select,
  Typography,
  Card,
  Tag,
} from 'antd';
import { useProjectsQuery } from '@help-line/entities/admin/query';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { UserChannels } from './user-channels';
import { notEmptyArrayValidator } from '@help-line/components';
import { UserMeta } from './user-meta';
import {
  CreateTicketRequest,
  HelpdeskAdminApi,
} from '@help-line/entities/admin/api';
import { useBoolean } from 'ahooks';
import { useApi } from '@help-line/modules/api';

export interface CreateTicketViewProps {
  lastCreatedTicket?: string;
}

export const CreateTicketView: React.FC<CreateTicketViewProps> = ({
  lastCreatedTicket,
}) => {
  const [form] = Form.useForm();
  const helpdeskApi = useApi(HelpdeskAdminApi);
  const [processing, processingActions] = useBoolean(false);
  const [ticketId, setTicketId] = useState(lastCreatedTicket || '');

  const projectsQuery = useProjectsQuery();
  const [languages, setLanguages] = useState<string[]>([]);
  const setLanguageByProject = useCallback(
    (value: string) => {
      const project = projectsQuery.data?.find((x) => x.id === value);
      if (project) {
        setLanguages(project.languages);
        const currentSelectedLanguage = form.getFieldValue('language');
        if (!project.languages.includes(currentSelectedLanguage)) {
          form.setFieldsValue({ language: '' });
        }
      }
    },
    [projectsQuery]
  );

  const onCreate = useCallback(
    async (values: CreateTicketRequest) => {
      processingActions.setTrue();
      try {
        const ticketId = await helpdeskApi.createTicket(values);
        message.success(`Ticket created, id: ${ticketId}`);
        form.resetFields();
        setTicketId(ticketId);
      } catch (e: any) {
        message.error(`Failed to create ticket, ${e.message}`);
      } finally {
        processingActions.setFalse();
      }
    },
    [processingActions]
  );

  return (
    <div
      className={cn(
        boxCss.flex,
        boxCss.flexColumn,
        boxCss.fullHeight,
        boxCss.overflowHidden,
        spacingCss.spaceSm
      )}
    >
      <Card className={cn(boxCss.fullWidth)} size={'small'}>
        <Form
          data-testid="submit-form"
          form={form}
          layout="vertical"
          onFinish={onCreate}
        >
          <div
            className={cn(
              boxCss.flex,
              boxCss.justifyContentSpaceBetween,
              boxCss.alignItemsCenter
            )}
          >
            <Typography.Title level={3} className={spacingCss.marginNone}>
              Create new ticket
            </Typography.Title>
            <div
              className={cn(
                boxCss.flex,
                spacingCss.spaceSm,
                boxCss.alignItemsCenter
              )}
            >
              {ticketId && (
                <div>
                  <Typography.Text>Last created ticket:</Typography.Text>
                  <Tag className={spacingCss.marginLeftSm}>
                    <Typography.Text copyable>{ticketId}</Typography.Text>
                  </Tag>
                </div>
              )}
              <Button type="primary" htmlType="submit" loading={processing}>
                Create
              </Button>
            </div>
          </div>
        </Form>
      </Card>
      <div
        className={cn(
          boxCss.flex,
          boxCss.fullWidth,
          boxCss.fullHeight,
          spacingCss.spaceMd,
          boxCss.overflowAuto
        )}
      >
        <Card className={boxCss.fullWidth} size={'small'} title="Main">
          <Form form={form} layout="vertical">
            <Form.Item
              label="Project"
              name="project"
              rules={[{ required: true, message: 'Please select project' }]}
            >
              <Select
                data-testid="project-selector"
                onChange={setLanguageByProject}
                disabled={processing}
                loading={projectsQuery.isLoading}
                placeholder={'Select project'}
              >
                {projectsQuery.data?.map((x) => (
                  <Select.Option key={x.id} value={x.id} disabled={!x.active}>
                    {x.info.name}
                  </Select.Option>
                ))}
              </Select>
            </Form.Item>
            <Form.Item
              label="Language"
              name="language"
              rules={[{ required: true, message: 'Please select language' }]}
            >
              <Select
                disabled={!form.getFieldValue('project') || processing}
                data-testid="language-selector"
                placeholder={'Select language'}
              >
                {languages.map((x) => (
                  <Select.Option key={x} value={x}>
                    {x.toUpperCase()}
                  </Select.Option>
                ))}
              </Select>
            </Form.Item>
            <Form.Item label="Tags" name="tags">
              <Select mode="tags" disabled={processing} />
            </Form.Item>
            <Form.Item
              label="Message"
              name="text"
              rules={[{ required: true, message: 'Message is required' }]}
            >
              <Input.TextArea disabled={processing} />
            </Form.Item>
          </Form>
        </Card>

        <div className={cn(boxCss.fullWidth)}>
          <Form form={form} layout="vertical" onFinish={onCreate}>
            <Form.List
              name="channels"
              rules={[
                {
                  validator: notEmptyArrayValidator,
                  message: 'Please add channels',
                },
              ]}
            >
              {(fields, operation, meta) => (
                <UserChannels
                  fields={fields}
                  meta={meta}
                  operation={operation}
                  disabled={processing}
                />
              )}
            </Form.List>
            <Form.List name="userMeta">
              {(fields, operation, meta) => (
                <div className={cn(spacingCss.marginTopMd)}>
                  <UserMeta
                    fields={fields}
                    meta={meta}
                    operation={operation}
                    disabled={processing}
                  />
                </div>
              )}
            </Form.List>
          </Form>
        </div>
      </div>
    </div>
  );
};
