import React, { MouseEvent, useCallback } from "react";
import cn from "classnames";
import { boxCss, spacingCss } from "@shared/styles";
import { ChannelItem } from "@entities/helpdesk";
import { Button, Form, Input, Space, Typography } from "antd";
import { FormListFieldData, FormListOperation } from "antd/lib/form/FormList";
import { DeleteOutlined, MinusCircleOutlined } from "@ant-design/icons";

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
      channel: "",
      userId: "",
    } as ChannelItem);
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
        <Typography.Text>Channels</Typography.Text>
        <Button size="small" onClick={onAdd} type="dashed" disabled={disabled}>
          Add
        </Button>
      </div>

      {fields.map(({ key, name, fieldKey, ...restField }) => (
        <Space key={key} className={boxCss.flex} align="baseline">
          <Form.Item
            {...restField}
            name={[name, "userId"]}
            fieldKey={[fieldKey, "userId"]}
            rules={[{ required: true, message: "Missing user id" }]}
          >
            <Input prefix="UserID:" disabled={disabled} />
          </Form.Item>
          <Form.Item
            {...restField}
            name={[name, "channel"]}
            fieldKey={[fieldKey, "channel"]}
            rules={[{ required: true, message: "Missing channel" }]}
          >
            <Input prefix="Channel: " disabled={disabled} />
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
        <div>
          <Form.ErrorList errors={meta.errors} />
        </div>
      </div>
    </div>
  );
};
