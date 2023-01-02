import React, { memo, useCallback, useMemo, useState } from 'react';
import { Tag } from '@help-line/entities/client/api';
import { Button, Divider, Input, Row, Tag as AntdTag, Typography } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import uniq from 'lodash/uniq';
import { useKeyPress } from 'ahooks';
import { useInput } from '@help-line/components';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';

export const TagCreateForm = memo(
  ({
    tags,
    className,
    onSave,
    onCancel,
    loading,
  }: {
    tags: Tag[];
    className?: string;
    onSave: (tags: string[]) => void;
    onCancel: () => void;
    loading?: boolean;
  }) => {
    const [value, onChange, setValue] = useInput('');
    const [items, setItems] = useState<string[]>([]);
    const handleValues = useCallback(() => {
      const tags = value
        .split(/[,;]/gi)
        .map((x) => x.trim())
        .filter((x) => !!x);
      setItems(uniq([...items, ...tags]));
      setValue('');
    }, [value, items, setValue]);

    useKeyPress('Enter', () => handleValues());

    const removeValue = useCallback(
      (item: string) => {
        setItems(items.filter((x) => x !== item));
      },
      [items]
    );

    const uniqueTags = useMemo(() => {
      return items.filter((x) => tags.every((t) => t.key !== x));
    }, [items, tags]);

    return (
      <div
        data-testid="tag-form"
        className={cn(
          className,
          boxCss.flex,
          boxCss.flexColumn,
          boxCss.fullHeight,
          boxCss.fullWidth
        )}
      >
        <Row justify="space-between" className={spacingCss.marginBottomSm}>
          <Typography.Text strong>Tags</Typography.Text>
        </Row>
        <Input
          value={value}
          onChange={onChange}
          placeholder="Enter tags"
          disabled={loading}
          suffix={
            <Button
              type="text"
              size="small"
              onClick={handleValues}
              disabled={loading}
              data-testid="add"
            >
              <PlusOutlined />
            </Button>
          }
        />
        <Typography.Text type="secondary">
          (inserting multiple elements is supported)
        </Typography.Text>
        <Divider />

        <div
          className={cn(
            boxCss.flex,
            boxCss.flexWrap,
            spacingCss.spaceXs,
            spacingCss.marginBottomLg
          )}
        >
          {items.map((x) => (
            <AntdTag
              key={x}
              closable={!loading}
              color={uniqueTags.includes(x) ? '' : 'red'}
              onClose={() => removeValue(x)}
              data-testid="tag"
            >
              {x}
            </AntdTag>
          ))}
        </div>

        <Row className={cn(spacingCss.spaceXs)} justify="end">
          <Button disabled={loading} onClick={onCancel}>
            Cancel
          </Button>
          <Button
            disabled={uniqueTags.length === 0}
            loading={loading}
            type="primary"
            onClick={() => onSave(uniqueTags)}
            data-testid="save"
          >
            Save
          </Button>
        </Row>
      </div>
    );
  }
);
