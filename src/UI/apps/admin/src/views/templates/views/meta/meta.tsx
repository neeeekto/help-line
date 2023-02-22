import React, { FormEvent, useCallback, useMemo } from 'react';
import { observer } from 'mobx-react-lite';
import { Input, Typography } from 'antd';
import { spacingCss, textCss } from '@help-line/style-utils';
import cn from 'classnames';
import { EditTab, Resource, ResourceType, useEditStore } from '../../state';
import { TemplateMeta } from './template.meta';
import { ContextMeta } from './context.meta';
import { makeGroupValueAccessor } from '../../state';

const MetaContent: React.FC<{
  className?: string;
  resource: Resource;
  tab: EditTab;
}> = observer(({ className, resource, tab }) => {
  const store$ = useEditStore();
  const groupValAccessor = useMemo(
    () => store$.createValueAccessor(makeGroupValueAccessor()),
    [store$]
  );

  const updateGroup = useCallback(
    (evt: FormEvent<HTMLInputElement>) => {
      groupValAccessor.set(evt.currentTarget.value);
    },
    [groupValAccessor]
  );

  return (
    <div className={cn(spacingCss.paddingSm, className)}>
      <Typography.Title level={4} className={textCss.right}>
        Meta
      </Typography.Title>
      <div>
        <Typography.Text strong>Group</Typography.Text>
      </div>
      <div className={spacingCss.marginTopSm}>
        <Input
          size="small"
          value={groupValAccessor.get()}
          onInput={updateGroup}
        />
      </div>
      {resource.type === ResourceType.Template && (
        <TemplateMeta tab={tab} resource={resource as any} />
      )}
      {resource.type === ResourceType.Context && (
        <ContextMeta resource={resource as any} tab={tab} />
      )}
    </div>
  );
});

export const Meta: React.FC<{ className?: string }> = observer(
  ({ className }) => {
    const store$ = useEditStore();
    const activeResource = store$.selectors.current();

    return activeResource ? (
      <MetaContent
        className={className}
        resource={activeResource.resource}
        tab={activeResource.tab}
      />
    ) : null;
  }
);
