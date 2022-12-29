import React, { FormEvent, useCallback, useMemo } from 'react';
import { EditedItem, SourceType } from '../../state/types';
import { Context, Template } from '@help-line/entities/admin/api';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { Button, Input, Popover, Select, Typography } from 'antd';
import cn from 'classnames';
import groupBy from 'lodash/groupBy';
import { editorStore } from '../../state/store';
import { observer } from 'mobx-react-lite';
import { compile } from 'handlebars';
import { Icon } from '../../components/Icon';
import {
  Resource,
  ResourceType,
  useOpenTabAction,
  useResourcesByTypeQuery,
  ValueAccessor,
} from '../../editor-manager';
import { makeTemplatePropsValueAccessor } from '../../editor-manager/value-accessors';

export const TemplateMeta: React.FC<{ resource: Resource }> = observer(
  ({ resource }) => {
    const contexts = useResourcesByTypeQuery<Context>(ResourceType.Context);
    const open = useOpenTabAction();

    const onEditProps = useCallback(() => {
      open({
        id: `${resource.id}_props`,
        resource: resource.type,
        language: 'json',
        breadcrumb: [resource.type, resource.id, 'props'],
        value: makeTemplatePropsValueAccessor() as ValueAccessor,
      });
    }, [open]);

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
            open={duplicateContextWithAlias.length > 0}
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
          <Button size="small" type="text" onClick={onEditProps}>
            Edit
          </Button>
        </div>
        <div className={spacingCss.marginTopSm}>
          {/*<ReactJson src={edit.current.props || {}} />*/}
        </div>
        <div className={spacingCss.marginTopLg}>
          <Typography.Text strong>Description</Typography.Text>
        </div>
        <div className={spacingCss.marginTopSm}>
          {/*<Select
            className={boxCss.fullWidth}
            value={edit.current.meta?.description}
            onChange={updateDescription}
          >
            {Object.keys(descriptions.data || {}).map((x) => (
              <Select.Option key={x} value={x}>
                {x}
              </Select.Option>
            ))}
          </Select>*/}
        </div>
      </>
    );
  }
);
