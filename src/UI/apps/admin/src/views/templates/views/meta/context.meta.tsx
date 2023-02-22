import React, { useCallback, useMemo } from 'react';
import { Context } from '@help-line/entities/admin/api';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { AutoComplete, Select, Tooltip, Typography } from 'antd';
import {
  EditTab,
  makeContextAliasValueAccessor,
  makeContextExtendValueAccessor,
  Resource,
  ResourceType,
  useEditStore,
} from '../../state';
import { observer } from 'mobx-react-lite';
import uniq from 'lodash/uniq';

export const ContextMeta = observer(
  ({ resource, tab }: { tab: EditTab; resource: Resource<Context> }) => {
    const store$ = useEditStore();
    const contextResources = store$.selectors.resourceByType<Context>(
      ResourceType.Context
    );
    const aliases = useMemo(() => {
      return uniq(
        contextResources
          .filter((x) => x.data.alias)
          .map((x) => x.data.alias!) || []
      ).map((x) => ({ value: x }));
    }, [contextResources]);

    const aliasAccessor = useMemo(
      () =>
        store$.createValueAccessor<Context>(makeContextAliasValueAccessor()),
      [store$]
    );

    const extendAccessor = useMemo(
      () =>
        store$.createValueAccessor<Context>(makeContextExtendValueAccessor()),
      [store$]
    );

    const updateAlias = useCallback(
      (val: string) => {
        aliasAccessor.set(val);
      },
      [aliasAccessor]
    );
    const updateExtend = useCallback(
      (contextId: string) => {
        extendAccessor.set(contextId);
      },
      [extendAccessor]
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
            value={aliasAccessor.get()}
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
            value={extendAccessor.get()}
          >
            {contextResources
              ?.filter((x) => x.data.id !== resource.data.id)
              .map((x) => (
                <Select.Option key={x.data.id} value={x.data.id}>
                  {x.data.id}
                </Select.Option>
              ))}
          </Select>
        </div>
      </>
    );
  }
);
