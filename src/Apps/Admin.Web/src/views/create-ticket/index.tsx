import React, { useCallback, useState } from "react";
import cn from "classnames";
import {
  Button,
  Form,
  Input,
  message,
  Row,
  Select,
  Space,
  Typography,
} from "antd";
import { useProjectsQuery } from "@entities/projects";
import { boxCss, spacingCss, utilsCss } from "@shared/styles";
import { UserChannels } from "@views/create-ticket/user-channels";
import { notEmptyArrayValidator } from "@shared/validators";
import { UserMeta } from "@views/create-ticket/user-meta";
import { CreateTicketRequest, helpdeskApi } from "@entities/helpdesk";
import { useBoolean } from "ahooks";

const CreateTicket: React.FC = () => {
  const [form] = Form.useForm();
  const [processing, processingActions] = useBoolean(false);
  const [ticketId, setTicketId] = useState("");

  const projectsQuery = useProjectsQuery();
  const [languages, setLanguages] = useState<string[]>([]);
  const setLanguageByProject = useCallback(
    (value: string) => {
      const project = projectsQuery.data?.find((x) => x.id === value);
      if (project) {
        setLanguages(project.languages);
        const currentSelectedLanguage = form.getFieldValue("language");
        if (!project.languages.includes(currentSelectedLanguage)) {
          form.setFieldsValue({ language: "" });
        }
      }
    },
    [projectsQuery]
  );

  const create = useCallback(
    async (values: CreateTicketRequest) => {
      console.log(values);
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
    <div className={cn(boxCss.flex, boxCss.fullWidth, spacingCss.spaceMd)}>
      <div
        className={cn(utilsCss.bgWhite, spacingCss.paddingLg, boxCss.fullWidth)}
      >
        <Form form={form} layout="vertical">
          <Form.Item
            label="Project"
            name="project"
            rules={[{ required: true, message: "Please select project" }]}
          >
            <Select onChange={setLanguageByProject} disabled={processing}>
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
            rules={[{ required: true, message: "Please select language" }]}
          >
            <Select disabled={!form.getFieldValue("project") || processing}>
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
            rules={[{ required: true, message: "Message is required" }]}
          >
            <Input.TextArea disabled={processing} />
          </Form.Item>
          <div
            className={cn(
              spacingCss.marginTopLg,
              boxCss.flex,
              boxCss.justifyContentSpaceBetween
            )}
          >
            <div>
              {ticketId && (
                <Typography.Text copyable>{ticketId}</Typography.Text>
              )}
            </div>
            <Button type="primary" htmlType="submit" loading={processing}>
              Create
            </Button>
          </div>
        </Form>
      </div>
      <div className={cn(boxCss.fullWidth)}>
        <Form form={form} layout="vertical" onFinish={create}>
          <Form.List
            name="channels"
            rules={[{ validator: notEmptyArrayValidator }]}
          >
            {(fields, operation, meta) => (
              <div className={cn(utilsCss.bgWhite, spacingCss.paddingLg)}>
                <UserChannels
                  fields={fields}
                  meta={meta}
                  operation={operation}
                  disabled={processing}
                />
              </div>
            )}
          </Form.List>
          <Form.List name="userMeta">
            {(fields, operation, meta) => (
              <div
                className={cn(
                  utilsCss.bgWhite,
                  spacingCss.paddingLg,
                  spacingCss.marginTopMd
                )}
              >
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
  );
};

export default CreateTicket;
