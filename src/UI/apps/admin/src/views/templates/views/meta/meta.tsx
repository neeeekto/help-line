import React, { FormEvent, FormEventHandler, useCallback } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Input, Select, Typography } from 'antd';
import { boxCss, spacingCss, textCss } from '@help-line/style-utils';
import cn from 'classnames';
import { editorStore } from '@views/templates/state/editor.store';
import { Opened, SourceType } from '@views/templates/state/editro.types';
import ReactJson from 'react-json-view';
import { Context, Template } from '@entities/templates';
import { useContextQueries } from '@entities/templates/queries';
import { TemplateMeta } from '@views/templates/views/meta/template.meta';
import { ContextMeta } from '@views/templates/views/meta/context.meta';

const MetaContent: React.FC<{ className?: string; active: Opened }> = observer(
  ({ className, active }) => {
    const edit = editorStore.getEditModelByOpened(active);

    const updateGroup = useCallback(
      (evt: FormEvent<HTMLInputElement>) => {
        editorStore.changeField(active, 'group', evt.currentTarget.value);
      },
      [active]
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
            value={edit.current.group}
            onInput={updateGroup}
          />
        </div>
        {active.src === SourceType.Template && (
          <TemplateMeta active={active as Opened<Template>} />
        )}
        {active.src === SourceType.Context && (
          <ContextMeta active={active as Opened<Context>} />
        )}
      </div>
    );
  }
);

export const Meta: React.FC<{ className?: string }> = observer(
  ({ className }) => {
    const active = editorStore.active.get();

    return active ? (
      <MetaContent className={className} active={active} />
    ) : null;
  }
);
