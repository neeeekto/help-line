import React, { MouseEvent, useCallback } from 'react';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { Button, Card, Form, Input, Space, Typography } from 'antd';
import { FormListFieldData, FormListOperation } from 'antd/lib/form/FormList';
import { DeleteOutlined } from '@ant-design/icons';
import { KeyValue } from '@help-line/entities/share';

export const UserMeta: React.FC<{
  fields: FormListFieldData[];
  operation: FormListOperation;
  meta: {
    errors: React.ReactNode[];
  };
  disabled?: boolean;
}> = ({ meta, fields, operation, disabled }) => {
  const onAdd = useCallback(() => {
    operation.add({ key: '', value: '' } as KeyValue<string, string>);
  }, [operation]);

  const onRemove = useCallback(
    (evt: MouseEvent<HTMLButtonElement>) => {
      operation.remove(Number(evt.currentTarget.value));
    },
    [operation]
  );
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
          <Typography.Text>Meta</Typography.Text>
          <Button
            size="small"
            onClick={onAdd}
            type="dashed"
            disabled={disabled}
            data-testid={'add_meta'}
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
              name={[name, 'key']}
              rules={[{ required: true, message: 'Missing key' }]}
            >
              <Input prefix="Key:" disabled={disabled} />
            </Form.Item>
            <Form.Item
              {...restField}
              name={[name, 'value']}
              rules={[{ required: true, message: 'Missing value' }]}
            >
              <Input prefix="Value:" disabled={disabled} />
            </Form.Item>
            <Button
              value={name}
              type="text"
              onClick={onRemove}
              disabled={disabled}
              data-testid={`delete_${name}`}
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
          <div>
            <Form.ErrorList errors={meta.errors} />
          </div>
        </div>
      </div>
    </Card>
  );
};
