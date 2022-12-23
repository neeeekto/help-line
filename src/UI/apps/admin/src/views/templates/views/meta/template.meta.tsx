import React, { FormEvent, useCallback, useMemo } from 'react';
import {
  EditedItem,
  Opened,
  SourceType,
} from '@views/templates/state/editro.types';
import { Context, Template } from '@entities/templates';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { Button, Input, Popover, Select, Typography } from 'antd';
import cn from 'classnames';
import groupBy from 'lodash/groupBy';
import ReactJson from 'react-json-view';
import { editorStore } from '@views/templates/state/editor.store';
import { observer } from 'mobx-react-lite';
import {
  useContextQueries,
  useTemplateDescription,
} from '@entities/templates/queries';
import { compile } from 'handlebars';
import { getMainFieldForSrc } from '@views/templates/utils/editor.utils';
import { Icon } from '@views/templates/components/Icon';

export const TemplateMeta: React.FC<{ active: Opened<Template> }> = observer(
  ({ active }) => {
    const descriptions = useTemplateDescription();
    const contextQuery = useContextQueries().useListQuery();
    const edit = editorStore.getEditModelByOpened(
      active
    ) as EditedItem<Template>;
    const toEditProps = useCallback(() => {
      editorStore.open(edit, 'props', 'json');
    }, [edit]);

    const usedContexts = useMemo(() => {
      return (
        contextQuery.data?.filter((x) =>
          edit.current.contexts.includes(x.id)
        ) || []
      );
    }, [edit.current.contexts, contextQuery]);

    const duplicateContextWithAlias = useMemo(() => {
      const group = groupBy(
        usedContexts.filter((x) => x.alias),
        'alias'
      );
      return Object.keys(group)
        .filter((x) => group[x].length > 1)
        .map((x) => ({ alias: x, contexts: group[x] }));
    }, [usedContexts]);

    const updateContext = useCallback(
      (contextsIds: string[]) => {
        editorStore.changeField(active, 'contexts', contextsIds);
      },
      [active]
    );

    const openContext = useCallback((context: Context) => {
      editorStore.open(
        {
          id: context.id,
          src: SourceType.Context,
          current: { ...context },
          original: context,
        },
        getMainFieldForSrc(SourceType.Context),
        'json'
      );
    }, []);

    const showPreview = useCallback(() => {
      console.log(compile(edit.current.content)({}));
      editorStore.open(
        edit,
        'preview',
        'handlebars',
        compile(edit.current.content)({})
      );
    }, [edit]);

    const updateName = useCallback(
      (evt: FormEvent<HTMLInputElement>) => {
        editorStore.changeField(active, 'name', evt.currentTarget.value);
      },
      [active]
    );

    const updateDescription = useCallback(
      (id: string) => {
        editorStore.changeField(
          active,
          'meta',
          Object.assign(edit.current.meta || {}, { description: id })
        );
      },
      [active]
    );

    return (
      <>
        <div className={spacingCss.marginTopLg}>
          <Typography.Text strong>Name</Typography.Text>
        </div>
        <div className={spacingCss.marginTopSm}>
          <Input size="small" value={edit.current.name} onInput={updateName} />
        </div>
        <div className={spacingCss.marginTopLg}>
          <Typography.Text strong>Actions</Typography.Text>
        </div>
        <div
          className={cn(
            spacingCss.marginTopSm,
            boxCss.flex,
            boxCss.justifyContentEnd
          )}
        >
          <Button size="small" onClick={showPreview}>
            Preview
          </Button>
        </div>
        <div className={spacingCss.marginTopLg}>
          <Typography.Text strong>Contexts</Typography.Text>
        </div>
        <div className={spacingCss.marginTopSm}>
          <Popover
            content={
              <div>
                <div>Please, check contexts and aliases</div>
                {duplicateContextWithAlias.map((x) => (
                  <div key={x.alias} className={spacingCss.marginTopXs}>
                    <Typography.Text
                      strong
                      className={spacingCss.marginRightSm}
                    >
                      {x.alias}
                    </Typography.Text>
                    {x.contexts.map((c) => (
                      <Button
                        size="small"
                        type="text"
                        key={`${c.alias}_${c.id}`}
                        onClick={() => openContext(c)}
                      >
                        <Icon type={SourceType.Context} />
                        {c.id}
                      </Button>
                    ))}
                  </div>
                ))}
              </div>
            }
            title="Duplicates aliases"
            placement="rightBottom"
            visible={duplicateContextWithAlias.length > 0}
          >
            <Select
              mode="multiple"
              size="small"
              className={boxCss.fullWidth}
              onChange={updateContext}
              value={(edit.current as Template).contexts}
            >
              {contextQuery.data?.map((x) => (
                <Select.Option key={x.id} value={x.id}>
                  {x.id}
                </Select.Option>
              ))}
            </Select>
          </Popover>
        </div>

        <div
          className={cn(
            spacingCss.marginTopLg,
            boxCss.flex,
            boxCss.justifyContentSpaceBetween
          )}
        >
          <Typography.Text strong>Props</Typography.Text>
          <Button size="small" type="text" onClick={toEditProps}>
            Edit
          </Button>
        </div>
        <div className={spacingCss.marginTopSm}>
          <ReactJson src={edit.current.props || {}} />
        </div>
        <div className={spacingCss.marginTopLg}>
          <Typography.Text strong>Description</Typography.Text>
        </div>
        <div className={spacingCss.marginTopSm}>
          <Select
            className={boxCss.fullWidth}
            value={edit.current.meta?.description}
            onChange={updateDescription}
          >
            {Object.keys(descriptions.data || {}).map((x) => (
              <Select.Option key={x} value={x}>
                {x}
              </Select.Option>
            ))}
          </Select>
        </div>
      </>
    );
  }
);
