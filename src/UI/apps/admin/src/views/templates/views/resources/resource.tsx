import React, { useCallback, useState, MouseEvent, useMemo } from 'react';
import { useBoolean } from 'ahooks';
import { Button, Collapse } from 'antd';
import css from './resources.module.scss';
import { AddNew } from './add-new';
import { ResourceItem } from './resource-item';
import { ResourceType, useEditStore } from '../../state';
import { createTemplateItem } from '../../utils/editor.utils';
import { observer } from 'mobx-react-lite';

export interface ResourceProps {
  type: ResourceType;
  lang: string;
  name: string;
}

export const Resource = observer(({ type, name, lang }: ResourceProps) => {
  const store$ = useEditStore();
  const resources = store$.selectors.resourceByType(type);
  const [active, setActive] = useState<string | string[]>('');
  const [showAddInput, showAddInputActions] = useBoolean(false);
  const onShowAddForm = useCallback(
    (evt: MouseEvent) => {
      evt.stopPropagation();
      showAddInputActions.setTrue();
      setActive(type);
    },
    [showAddInputActions, type]
  );

  const onAdd = useCallback(
    (id: string) => {
      const item = createTemplateItem(id, type);
      store$.actions.addResource([item], type, true);
      showAddInputActions.setFalse();
    },
    [store$]
  );

  return (
    <Collapse
      ghost
      activeKey={active}
      onChange={setActive}
      className={css.collapseBox}
    >
      <Collapse.Panel
        header={name}
        key={type}
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
        {resources.map((x) => (
          <ResourceItem key={x.id} lang={lang} resource={x} />
        ))}
        {showAddInput && (
          <AddNew
            type={type}
            resources={resources ?? []}
            onAdd={onAdd}
            onCancel={showAddInputActions.setFalse}
          />
        )}
      </Collapse.Panel>
    </Collapse>
  );
});
