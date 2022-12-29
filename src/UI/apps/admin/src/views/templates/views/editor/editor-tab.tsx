import React, { useCallback, useEffect, useRef } from 'react';
import css from './editor.module.scss';
import cn from 'classnames';
import { Tooltip } from 'antd';
import { CloseOutlined } from '@ant-design/icons';
import { Icon } from '../../components/Icon';
import { spacingCss, textCss } from '@help-line/style-utils';
import { EditTab, useEditStore } from '../../state';
import { observer } from 'mobx-react-lite';

export const EditorTab = observer(({ tab }: { tab: EditTab }) => {
  const store$ = useEditStore();
  const ref = useRef<HTMLDivElement>(null);
  const current = store$.selectors.current();
  const resource = store$.selectors.resourceByTab(tab);
  const onSelect = useCallback(() => {
    store$.actions.openTab(tab);
  }, [tab, store$]);

  const onClose = useCallback(() => {
    store$.actions.closeTab(tab.id);
  }, [tab, close]);

  useEffect(() => {
    if (tab.id === current?.tab?.id) {
      ref.current?.scrollIntoView();
    }
  }, [current?.tab]);

  return (
    <div
      ref={ref}
      className={cn(css.tab, {
        [css.tabActive]: current?.tab?.id === tab.id,
      })}
    >
      <Tooltip title={tab.breadcrumb.join('.')} mouseEnterDelay={1.5}>
        <button
          className={cn(css.tabButton, textCss.truncate)}
          onClick={onSelect}
        >
          <span className={spacingCss.marginRightSm}>
            <Icon type={resource?.type} />
          </span>
          {resource?.data?.id}.{resource?.type}
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
