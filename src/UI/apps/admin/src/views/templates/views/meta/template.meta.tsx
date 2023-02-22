import React, { FormEvent, useCallback, useMemo } from 'react';
import { Component, Context, Template } from '@help-line/entities/admin/api';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { Button, Input, Popover, Select, Typography } from 'antd';
import cn from 'classnames';
import { JSONTree } from 'react-json-tree';
import groupBy from 'lodash/groupBy';
import { observer } from 'mobx-react-lite';
import { compile, registerPartial } from 'handlebars';
import { Icon } from '../../components/Icon';
import {
  makeTemplatePropsValueAccessor,
  useEditStore,
  EditTab,
  Resource,
  ResourceType,
  ValueAccessor,
  makeContextDataValueAccessor,
} from '../../state';

export const TemplateMeta = observer(
  ({ resource, tab }: { resource: Resource<Template>; tab: EditTab }) => {
    const store$ = useEditStore();
    const contexts = store$.selectors.resourceByType<Context>(
      ResourceType.Context
    );

    const onEditProps = useCallback(() => {
      store$.actions.openTab({
        id: `${resource.id}.props`,
        resource: resource.id,
        title: `${resource.data.id}.props`,
        language: 'json',
        breadcrumb: [resource.type, resource.data.id, 'props'],
        value: makeTemplatePropsValueAccessor() as ValueAccessor,
      });
    }, [open, resource]);

    const usedContexts = useMemo(() => {
      const current = store$.edit.get(resource, 'contexts');
      return contexts.filter((x) => current?.includes(x.id)) || [];
    }, [resource, contexts]);

    const duplicateContextWithAlias = useMemo(() => {
      const group = groupBy(
        usedContexts.filter((x) => x.data.alias),
        ['data', 'alias']
      );
      return Object.keys(group)
        .filter((x) => group[x].length > 1)
        .map((x) => ({ alias: x, contexts: group[x] }));
    }, [usedContexts]);

    const updateContext = useCallback(
      (contextsIds: string[]) => {
        store$.edit.set(resource, 'contexts', contextsIds);
      },
      [store$, resource]
    );

    const openContext = useCallback(
      (context: Context) => {
        const valAccessor = makeContextDataValueAccessor();
        store$.actions.openTab({
          id: `${context.id}.${valAccessor.field}`,
          resource: resource.id,
          value: valAccessor as ValueAccessor,
          breadcrumb: [resource.type, resource.data.id, 'context'],
          language: 'json',
        });
      },
      [store$, resource]
    );

    const showPreview = useCallback(() => {
      store$.actions.openTab({
        id: `${resource.id}.preview`,
        title: `preview: ${resource.id}`,
        resource: resource.id,
        value: {
          field: 'preview',
          set: () => ({}),
          get: (rsc, current) => {
            try {
              const compiler = compile(current?.content || rsc?.content || '');
              const components = store$.selectors.resourceByType<Component>(
                ResourceType.Component
              );

              return compiler(
                {},
                {
                  partials: components.reduce((res, c) => {
                    res[c.data.id] = c.data.content;
                    return res;
                  }, {} as any),
                }
              );
            } catch (e) {
              return e.message;
            }
          },
        } as ValueAccessor<Template> as any,
        breadcrumb: [resource.type, resource.data.id, 'preview'],
        language: 'html',
        readonly: true,
      });
    }, [store$, resource]);

    const updateName = useCallback(
      (evt: FormEvent<HTMLInputElement>) => {
        store$.edit.set(resource, 'name', evt.currentTarget.value);
      },
      [store$, resource]
    );

    const updateDescription = useCallback(
      (id: string) => {
        store$.edit.set(
          resource,
          'meta',
          Object.assign(store$.edit.get(resource, 'meta') || {}, {
            description: id,
          })
        );
      },
      [store$, resource]
    );

    return (
      <>
        <div className={spacingCss.marginTopLg}>
          <Typography.Text strong>Name</Typography.Text>
        </div>
        <div className={spacingCss.marginTopSm}>
          <Input
            size="small"
            value={store$.edit.get(resource, 'name')}
            onInput={updateName}
          />
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
                        key={`${c.data.alias}_${c.id}`}
                        onClick={() => openContext(c.data)}
                      >
                        <Icon type={ResourceType.Context} />
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
              value={store$.edit.get(resource, 'contexts')}
            >
              {contexts?.map((x) => (
                <Select.Option key={x.data.id} value={x.data.id}>
                  {x.data.id}
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
          <JSONTree
            theme={{ extend: 'default' }}
            data={store$.edit.get(resource, 'props')}
          ></JSONTree>
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
