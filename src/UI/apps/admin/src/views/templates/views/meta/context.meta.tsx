import React, { FormEvent, useCallback, useMemo, useState } from 'react';
import { EditedItem, Opened } from '../../state/types';
import { Context, Template } from '@help-line/entities/admin/api';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { AutoComplete, Input, Select, Tooltip, Typography } from 'antd';
import { editorStore } from '../../state/store';
import { observer } from 'mobx-react-lite';
import { useContextsQuery } from '@help-line/entities/admin/query';
import uniq from 'lodash/uniq';

export const ContextMeta: React.FC<{ active: Opened<Context> }> = observer(
  ({ active }) => {
    const edit = editorStore.getEditModelByOpened(
      active
    ) as EditedItem<Context>;
    const contextQuery = useContextsQuery();
    const aliases = useMemo(() => {
      return uniq(
        contextQuery.data?.filter((x) => x.alias).map((x) => x.alias!) || []
      ).map((x) => ({ value: x }));
    }, [contextQuery]);

    const updateAlias = useCallback(
      (val: string) => {
        editorStore.changeField(active, 'alias', val);
      },
      [active]
    );
    const updateExtend = useCallback(
      (contextsId: string) => {
        editorStore.changeField(active, 'extend', contextsId);
      },
      [active]
    );
    return (
      <>
        <div className={spacingCss.marginTopLg}>
          <Tooltip
            mouseEnterDelay={1}
            title={`Alias for this context. If you set this property than context will be available by two keys, context id and context alias.
            Warning! Two context with the same alias won't be merged, last defined in template (context property) will be used!`}
          >
            <Typography.Text strong>Alias</Typography.Text>
            <Typography.Text
              type="secondary"
              className={spacingCss.marginLeftXs}
            >
              (Optional)
            </Typography.Text>
          </Tooltip>
        </div>
        <div className={spacingCss.marginTopSm}>
          <AutoComplete
            className={boxCss.fullWidth}
            value={edit.current.alias}
            options={aliases}
            onChange={updateAlias}
          />
        </div>
        <div className={spacingCss.marginTopLg}>
          <Tooltip
            mouseEnterDelay={1}
            title="All property from target context will be merge with this context. You can use it for replace some properties: Languages, Themes, etc"
          >
            <Typography.Text strong>Extend</Typography.Text>
            <Typography.Text
              type="secondary"
              className={spacingCss.marginLeftXs}
            >
              (Optional)
            </Typography.Text>
          </Tooltip>
        </div>
        <div className={spacingCss.marginTopSm}>
          <Select
            size="small"
            className={boxCss.fullWidth}
            onChange={updateExtend}
            allowClear
            value={(edit.current as Context).extend}
          >
            {contextQuery.data
              ?.filter((x) => x.id !== active.id)
              .map((x) => (
                <Select.Option key={x.id} value={x.id}>
                  {x.id}
                </Select.Option>
              ))}
          </Select>
        </div>
      </>
    );
  }
);
