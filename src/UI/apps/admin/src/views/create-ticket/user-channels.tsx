import React, { MouseEvent, useCallback, useMemo } from 'react';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { ChannelItem } from '@help-line/entities/admin/api';
import { Button, Card, Form, Input, Select, Space, Typography } from 'antd';
import { FormListFieldData, FormListOperation } from 'antd/lib/form/FormList';
import { DeleteOutlined } from '@ant-design/icons';
import { Channels } from '@help-line/entities/client/api';
import { Rule } from 'rc-field-form/lib/interface';

export const UserChannels: React.FC<{
  fields: FormListFieldData[];
  operation: FormListOperation;
  meta: {
    errors: React.ReactNode[];
  };
  disabled?: boolean;
}> = ({ meta, fields, operation, disabled }) => {
  const onAdd = useCallback(() => {
    operation.add({
      channel: Channels.Email,
      userId: '',
    } as ChannelItem);
  }, [operation]);

  const onRemove = useCallback(
    (evt: MouseEvent<HTMLButtonElement>) => {
      operation.remove(Number(evt.currentTarget.value));
    },
    [operation]
  );

  const fieldsRules = useMemo(() => {
    return {
      channel: [{ required: true, message: 'Missing channel' }] as Rule[],
      userId: [{ required: true, message: 'Missing user id' }] as Rule[],
      userIdEmail: [
        { required: true, message: 'Missing user id' },
        { type: 'email', message: 'Incorrect email' },
      ] as Rule[],
    };
  }, []);

  const form = Form.useFormInstance();

  return (
    <Card
      className={boxCss.fullWidth}
      size={'small'}
      title={
        <div
          className={cn(
            boxCss.flex,
            boxCss.alignItemsCenter,
            boxCss.justifyContentSpaceBetween
          )}
        >
          <Typography.Text>Channels</Typography.Text>
          <Button
            size="small"
            onClick={onAdd}
            type="dashed"
            disabled={disabled}
          >
            Add
          </Button>
        </div>
      }
    >
      <div className={spacingCss.marginBottomSm}>
        {fields.map(({ key, name, fieldKey, ...restField }) => (
          <Space key={key} className={boxCss.flex} align="baseline">
            <Form.Item
              {...restField}
              name={[name, 'channel']}
              rules={fieldsRules.channel}
            >
              <Select
                disabled={disabled}
                placeholder="Select channel"
                style={{ minWidth: 100 }}
              >
                {Object.entries(Channels).map(([key, value]) => (
                  <Select.Option key={key} value={value}>
                    {key}
                  </Select.Option>
                ))}
              </Select>
            </Form.Item>
            <Form.Item
              {...restField}
              name={[name, 'userId']}
              rules={fieldsRules.userId}
            >
              <Input prefix="UserID:" disabled={disabled} />
            </Form.Item>
            <Button
              value={name}
              type="text"
              onClick={onRemove}
              className={boxCss.flex00Auto}
              disabled={disabled}
            >
              <DeleteOutlined />
            </Button>
          </Space>
        ))}
        <div
          className={cn(
            boxCss.flex,
            boxCss.alignItemsCenter,
            boxCss.justifyContentSpaceBetween
          )}
        >
          <Typography.Text type={'danger'}>
            <Form.ErrorList errors={meta.errors} />
          </Typography.Text>
        </div>
      </div>
    </Card>
  );
};
