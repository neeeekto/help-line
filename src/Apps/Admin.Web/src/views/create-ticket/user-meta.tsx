import React, { MouseEvent, useCallback } from "react";
import cn from "classnames";
import { boxCss, spacingCss } from "@shared/styles";
import { ChannelItem } from "@entities/helpdesk";
import { Button, Form, Input, Space, Typography } from "antd";
import { FormListFieldData, FormListOperation } from "antd/lib/form/FormList";
import { DeleteOutlined, MinusCircleOutlined } from "@ant-design/icons";
import { KeyValue } from "@entities/common";

export const UserMeta: React.FC<{
  fields: FormListFieldData[];
  operation: FormListOperation;
  meta: {
    errors: React.ReactNode[];
  };
  disabled?: boolean;
}> = ({ meta, fields, operation, disabled }) => {
  const onAdd = useCallback(() => {
    operation.add({ key: "", value: "" } as KeyValue<string, string>);
  }, [operation]);

  const onRemove = useCallback(
    (evt: MouseEvent<HTMLButtonElement>) => {
      operation.remove(Number(evt.currentTarget.value));
    },
    [operation]
  );
  return (
    <div className={spacingCss.marginBottomSm}>
      <div
        className={cn(
          spacingCss.marginBottomSm,
          boxCss.flex,
          boxCss.alignItemsCenter,
          boxCss.justifyContentSpaceBetween
        )}
      >
        <Typography.Text>Meta</Typography.Text>
        <Button size="small" onClick={onAdd} type="dashed" disabled={disabled}>
          Add
        </Button>
      </div>

      {fields.map(({ key, name, fieldKey, ...restField }) => (
        <Space key={key} className={boxCss.flex} align="baseline">
          <Form.Item
            {...restField}
            name={[name, "key"]}
            fieldKey={[fieldKey, "key"]}
            rules={[{ required: true, message: "Missing key" }]}
          >
            <Input prefix="Key:" disabled={disabled} />
          </Form.Item>
          <Form.Item
            {...restField}
            name={[name, "value"]}
            fieldKey={[fieldKey, "value"]}
            rules={[{ required: true, message: "Missing value" }]}
          >
            <Input prefix="Value:" disabled={disabled} />
          </Form.Item>
          <Button
            value={name}
            type="text"
            onClick={onRemove}
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
        <div>
          <Form.ErrorList errors={meta.errors} />
        </div>
      </div>
    </div>
  );
};
