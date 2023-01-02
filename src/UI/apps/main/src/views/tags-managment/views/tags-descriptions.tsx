import React, { ChangeEvent, useCallback, useEffect, useState } from 'react';
import first from 'lodash/first';
import { useDebounce } from 'ahooks';
import {
  Button,
  Empty,
  Input,
  Popconfirm,
  Row,
  Select,
  Space,
  Spin,
  Switch,
  Table,
} from 'antd';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { PlusOutlined, SearchOutlined } from '@ant-design/icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { TagDescription } from '@help-line/entities/client/api';
import { Project } from '@help-line/entities/admin/api';
import {
  useDeleteTagDescriptionMutation,
  useSaveTagDescriptionMutation,
  useSystemSettingsQuery,
  useTagDescriptionListQuery,
} from '@help-line/entities/client/query';
import {
  FullPageContainer,
  OnOffSelector,
  useDataFilter,
} from '@help-line/components';
import { TagDescriptionIssues } from '../components/tag-description-issues';

const TagDescriptionActions = ({
  description,
  projectId,
}: {
  description: TagDescription;
  projectId: Project['id'];
}) => {
  const removeMutation = useDeleteTagDescriptionMutation({
    id: description.tag,
    projectId,
  });
  const saveMutation = useSaveTagDescriptionMutation({
    id: description.tag,
    projectId,
  });
  const onRemove = useCallback(() => removeMutation.mutate(), [removeMutation]);
  const onToggle = useCallback(
    () =>
      saveMutation.mutate({
        ...description,
        enabled: !description.enabled,
      }),
    [removeMutation, description]
  );

  return (
    <Space wrap={false} align="center">
      <Switch
        size="small"
        checked={description.enabled}
        loading={saveMutation.isLoading}
        disabled={removeMutation.isLoading}
        onChange={onToggle}
      />
      <Popconfirm
        placement="topLeft"
        title="Are you sure delete this item?"
        onConfirm={onRemove}
        okText="Yes"
        cancelText="No"
        disabled={removeMutation.isLoading || saveMutation.isLoading}
      >
        <Button
          size="small"
          type="text"
          loading={removeMutation.isLoading}
          disabled={saveMutation.isLoading}
        >
          <FontAwesomeIcon icon="trash" />
        </Button>
      </Popconfirm>
    </Space>
  );
};

export const TagsDescriptions = ({
  projectId,
}: {
  projectId: Project['id'];
}) => {
  const systemSettings = useSystemSettingsQuery();
  const listQuery = useTagDescriptionListQuery({ projectId });

  const [search, setSearch] = useState('');
  const [status, setStatus] = useState<boolean | null>(null);
  const [language, setLanguage] = useState('');

  useEffect(() => {
    setLanguage(
      systemSettings.data?.defaultLanguage ||
        first(systemSettings.data?.languages) ||
        ''
    );
  }, [systemSettings.data]);
  const debouncedSearch = useDebounce(search, { wait: 500 });
  const onSearch = useCallback(
    (event: ChangeEvent<HTMLInputElement>) => setSearch(event.target.value),
    []
  );
  const filteredList = useDataFilter(
    listQuery.data || [],
    [
      (x) => status === null || x.enabled === status,
      (x, search) =>
        !search ||
        x.tag.toLowerCase().includes(search) ||
        x.issues.some((i) =>
          Object.values(i.contents).some(
            (c) =>
              c.text.toLowerCase().includes(search) ||
              c.uri?.toLowerCase().includes(search)
          )
        ),
    ],
    [status, debouncedSearch.toLowerCase().trim()]
  );

  if (listQuery.isLoading) {
    return (
      <FullPageContainer useCenterPlacement>
        <Spin />
      </FullPageContainer>
    );
  }

  if (!listQuery.data) {
    return <Empty />;
  }

  return (
    <FullPageContainer className={cn(boxCss.flex, boxCss.flexColumn)}>
      <Row justify="space-between" className={boxCss.flex00Auto}>
        <Space>
          <Input
            value={search}
            placeholder="Search"
            onChange={onSearch}
            suffix={<SearchOutlined />}
          />
          <OnOffSelector value={status} onChange={setStatus} />
          <Select value={language} onSelect={setLanguage}>
            {systemSettings.data?.languages?.map((x) => (
              <Select.Option key={x} value={x}>
                {x.toUpperCase()}
              </Select.Option>
            ))}
          </Select>
        </Space>
        <Button>
          <PlusOutlined />
          Add
        </Button>
      </Row>
      <Table
        dataSource={filteredList!}
        pagination={false}
        size="small"
        showHeader={false}
        className={cn(
          boxCss.fullHeight,
          boxCss.overflowAuto,
          spacingCss.marginTopLg
        )}
      >
        <Table.Column key="tag" dataIndex="tag" />
        <Table.Column
          key="issues"
          width="100%"
          render={(desc) => (
            <TagDescriptionIssues description={desc} viewLanguage={language} />
          )}
        />
        <Table.Column
          key="enabled"
          render={(desc: TagDescription) => (
            <TagDescriptionActions projectId={projectId} description={desc} />
          )}
        />
      </Table>
    </FullPageContainer>
  );
};
