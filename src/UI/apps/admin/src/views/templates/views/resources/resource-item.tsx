import React, { useCallback, useMemo } from 'react';
import { Button, Popconfirm } from 'antd';
import css from './resources.module.scss';
import { DeleteOutlined, SaveOutlined, StopOutlined } from '@ant-design/icons';
import { spacingCss, textCss } from '@help-line/style-utils';
import cn from 'classnames';
import { Icon } from '../../components/Icon';
import {
  Resource,
  makeValueAccessorByResource,
  useEditStore,
  ValueAccessor,
} from '../../state';
import { observer } from 'mobx-react-lite';
import {
  useDeleterResourceMutation,
  useSaveResourceMutation,
} from '../../state/hooks';

export const ResourceItem = observer(
  ({ resource, lang }: { resource: Resource; lang: string }) => {
    const store$ = useEditStore();

    const deleteMutation = useDeleterResourceMutation(resource.id);
    const saveMutation = useSaveResourceMutation(resource.id);

    const onOpen = useCallback(() => {
      const valAccessor = makeValueAccessorByResource(resource.type);

      store$.actions.openTab({
        id: `${resource.id}.${valAccessor.field}`,
        resource: resource.id,
        value: valAccessor as ValueAccessor,
        breadcrumb: [resource.type, resource.data.id, valAccessor.field || ''],
        language: lang,
      });
    }, [store$]);

    const onDeleteHandler = useCallback(async () => {
      deleteMutation.mutate();
    }, [store$]);

    const onSaveHandler = useCallback(async () => {
      saveMutation.mutate();
    }, [saveMutation]);

    const onReset = useCallback(async () => {
      store$.actions.clearEditing(resource.id);
    }, [store$]);

    const isChanged = store$.selectors.isChanged(resource.id);
    const active = store$.selectors.current()?.tab.resource === resource.id;

    return (
      <div
        className={cn(css.item, {
          [css.itemSelected]: active,
        })}
      >
        <button
          className={cn(css.itemSelectButton, textCss.truncate, {
            [css.itemSelectButtonNew]: resource.isNew,
            [css.itemSelectButtonChanged]: isChanged,
          })}
          onClick={onOpen}
        >
          <span className={spacingCss.marginRightMd}>
            <Icon type={resource.type} />
          </span>
          {resource.data.id}
        </button>
        {isChanged && (
          <Button
            type="text"
            size="small"
            className={css.itemDelButton}
            onClick={onSaveHandler}
            loading={saveMutation.isLoading}
          >
            <SaveOutlined />
          </Button>
        )}
        {isChanged && !resource.isNew && (
          <Button
            type="text"
            size="small"
            className={css.itemDelButton}
            onClick={onReset}
            disabled={saveMutation.isLoading}
          >
            <StopOutlined />
          </Button>
        )}

        <Popconfirm title="Are you sure?" onConfirm={onDeleteHandler}>
          <Button
            type="text"
            size="small"
            className={css.itemDelButton}
            icon={<DeleteOutlined />}
            loading={deleteMutation.isLoading}
          ></Button>
        </Popconfirm>
      </div>
    );
  }
);
