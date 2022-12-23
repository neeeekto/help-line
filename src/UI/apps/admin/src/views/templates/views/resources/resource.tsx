import React, { useCallback, useState, MouseEvent } from 'react';
import { observer } from 'mobx-react-lite';
import { useBoolean } from 'ahooks';
import { Button, Collapse, Input } from 'antd';
import { editorStore } from '../../state/editor.store';
import css from './resources.module.scss';
import { AddNew } from './add-new';
import { ResourceItem } from './resource-item';
import { SourceType } from '../../state/editro.types';
import {
  createTemplateItem,
  getMainFieldForSrc,
} from '../../utils/editor.utils';
import { UseQueryResult } from '@tanstack/react-query';
import { TemplateBase } from '@help-line/entities/admin/api';

export interface ResourceProps {
  src: SourceType;
  lang: string;
  name: string;
  queryList: UseQueryResult<TemplateBase[]>;
}

export const Resource: React.FC<ResourceProps> = observer(
  ({ src: src, name, lang }) => {
    const [active, setActive] = useState<string | string[]>('');
    const [showAddInput, showAddInputActions] = useBoolean(false);
    const query = queries.useListQuery();
    const onShowAddForm = useCallback(
      (evt: MouseEvent) => {
        evt.stopPropagation();
        showAddInputActions.setTrue();
        setActive(src);
      },
      [showAddInputActions, src]
    );

    const onAdd = useCallback((value: string) => {
      const item = createTemplateItem(value, src);
      editorStore.open(
        {
          current: item,
          src,
          id: item.id,
        },
        getMainFieldForSrc(src),
        lang
      );
      showAddInputActions.setFalse();
    }, []);

    return (
      <Collapse
        ghost
        activeKey={active}
        onChange={setActive}
        className={css.collapseBox}
      >
        <Collapse.Panel
          header={name}
          key={src}
          extra={
            <Button
              type="text"
              size="small"
              className={css.addButton}
              onClick={onShowAddForm}
              disabled={showAddInput}
            >
              + Add
            </Button>
          }
        >
          {query.data?.map((x) => (
            <ResourceItem
              key={x.id}
              item={x}
              lang={lang}
              src={src}
              original={x}
              queries={queries}
            />
          ))}
          {editorStore.state.edited
            .filter((x) => x.src === src && !x.original)
            .map((x) => (
              <ResourceItem
                key={x.id}
                item={x.current}
                lang={lang}
                src={src}
                queries={queries}
              />
            ))}
          {showAddInput && (
            <AddNew
              type={src}
              items={query.data ?? []}
              onAdd={onAdd}
              onCancel={showAddInputActions.setFalse}
            />
          )}
        </Collapse.Panel>
      </Collapse>
    );
  }
);
