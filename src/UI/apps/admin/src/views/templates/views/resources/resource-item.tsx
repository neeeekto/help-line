import React, { useCallback, useMemo } from 'react';
import { TemplateBase } from '@help-line/entities/admin/api';
import { Button, Popconfirm, Tag } from 'antd';
import css from './resources.module.scss';
import { DeleteOutlined, SaveOutlined, StopOutlined } from '@ant-design/icons';
import { spacingCss, textCss } from '@help-line/style-utils';
import { observer } from 'mobx-react-lite';
import cn from 'classnames';
import { editorStore } from '../../state/editor.store';
import { SourceType } from '../../state/editro.types';
import { Icon } from '../../components/Icon';
import { getMainFieldForSrc } from '../../utils/editor.utils';
import { useKeyPress } from 'ahooks';
import { UseMutationResult } from '@tanstack/react-query';

export const ResourceItem: React.FC<{
  item: TemplateBase;
  original?: TemplateBase;
  lang: string;
  src: SourceType;
  deleteMutationFactory: (id: TemplateBase['id']) => UseMutationResult;
  updateMutationFactory: (id: TemplateBase['id']) => UseMutationResult;
}> = observer(
  ({
    item,
    lang,
    src: src,
    original,
    deleteMutationFactory,
    updateMutationFactory,
  }) => {
    const deleteMutation = deleteMutationFactory(item.id);
    const saveMutation = updateMutationFactory(item.id);
    const onOpen = useCallback(() => {
      editorStore.open(
        {
          id: item.id,
          src: src,
          current: { ...item },
          original,
        },
        getMainFieldForSrc(src),
        lang
      );
    }, [item, lang, src]);

    const onDeleteHandler = useCallback(async () => {
      if (original) {
        await deleteMutation.mutateAsync(item.id);
      }
      editorStore.remove(item.id, src);
    }, [item, src]);

    const editing = editorStore.state.edited.find((x) =>
      editorStore.isEqual(x, { id: item.id, src: src })
    );

    const onSaveHandler = useCallback(async () => {
      if (editing) {
        await saveMutation.mutateAsync(editing.current);
        editorStore.markAsSaved(editing);
      }
    }, [editing]);

    const onReset = useCallback(async () => {
      if (editing && original) {
        editorStore.reset(item.id, src, original);
      }
    }, [editing]);

    const isNew = editing && !editing.original;
    const isChanged =
      editing?.current.updatedAt !== editing?.original?.updatedAt;
    const active = editorStore.active.get();

    const isActive = useMemo(() => {
      return active?.id === item.id && active?.src === src;
    }, [active, item]);

    useKeyPress(['shift.s'], (evt) => {
      if (isActive && isChanged) {
        evt.stopPropagation();
        onSaveHandler();
      }
    });
    useKeyPress(['ctrl.shift.s'], (evt) => {
      if (isChanged) {
        evt.stopPropagation();
        onSaveHandler();
      }
    });
    return (
      <div
        className={cn(css.item, {
          [css.itemSelected]: isActive,
        })}
      >
        <button
          className={cn(css.itemSelectButton, textCss.truncate, {
            [css.itemSelectButtonNew]: isNew,
            [css.itemSelectButtonChanged]: isChanged,
          })}
          onClick={onOpen}
        >
          <span className={spacingCss.marginRightMd}>
            <Icon type={src} />
          </span>
          {item.id}
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
        {isChanged && original && (
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
            loading={deleteMutation.isLoading}
          >
            <DeleteOutlined />
          </Button>
        </Popconfirm>
      </div>
    );
  }
);
