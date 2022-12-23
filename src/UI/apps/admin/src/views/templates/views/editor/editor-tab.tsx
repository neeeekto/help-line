import React, { useCallback, useEffect, useRef } from 'react';
import { observer } from 'mobx-react-lite';
import { Opened } from '../../state/editro.types';
import css from './editor.module.scss';
import cn from 'classnames';
import { editorStore } from '../../state/editor.store';
import { Tooltip } from 'antd';
import { CloseOutlined } from '@ant-design/icons';
import { Icon } from '../../components/Icon';
import { spacingCss, textCss } from '@help-line/style-utils';

export const EditorTab: React.FC<{ item: Opened }> = observer(({ item }) => {
  const ref = useRef<HTMLDivElement>(null);
  const onSelect = useCallback(() => {
    editorStore.focus(item);
  }, [item]);

  const onClose = useCallback(() => {
    editorStore.close(item);
  }, [item]);

  useEffect(() => {
    if (item.active && ref.current) {
      ref.current.scrollIntoView();
    }
  }, [item.active]);

  return (
    <div
      ref={ref}
      className={cn(css.tab, {
        [css.tabActive]: item.active,
      })}
    >
      <Tooltip title={[item.id, item.src].join('.')} mouseEnterDelay={1.5}>
        <button
          className={cn(css.tabButton, textCss.truncate)}
          onClick={onSelect}
        >
          <span className={spacingCss.marginRightSm}>
            <Icon type={item.src} field={item.field} />
          </span>
          {item.id}.{item.src}
        </button>
      </Tooltip>

      <button
        className={cn(css.tabButton, css.tabButtonClose)}
        onClick={onClose}
      >
        <CloseOutlined style={{ fontSize: '12px' }} />
      </button>
    </div>
  );
});
