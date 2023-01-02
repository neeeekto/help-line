import React, { useCallback, useState } from 'react';
import { Project, Tag } from '@help-line/entities/client/api';
import {
  Spin,
  Table,
  Row,
  Input,
  Space,
  Button,
  Switch,
  Popconfirm,
  Empty,
  Drawer,
} from 'antd';
import { useBoolean } from 'ahooks';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { PlusOutlined, SearchOutlined } from '@ant-design/icons';
import { TagCreateForm } from '../components/tag-create-form';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';
import {
  useDeleteTagMutation,
  useSaveManyTagsMutation,
  useSaveTagMutation,
  useTagListQuery,
} from '@help-line/entities/client/query';
import {
  OnOffSelector,
  useDataFilter,
  useDebounceInput,
} from '@help-line/components';

const TagActions = ({
  tag,
  projectId,
}: {
  tag: Tag;
  projectId: Project['id'];
}) => {
  const removeMutation = useDeleteTagMutation({
    projectId: projectId,
    id: tag.key,
  });
  const saveMutation = useSaveTagMutation({ id: tag.key, projectId });
  const onRemove = useCallback(
    () => removeMutation.mutate(),
    [tag, removeMutation]
  );

  return (
    <Space wrap={false} align="center">
      <Switch
        size="small"
        checked={tag.enabled}
        loading={saveMutation.isLoading}
        disabled={removeMutation.isLoading}
        onChange={() => saveMutation.mutate(!tag.enabled)}
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

export const Tags = ({ projectId }: { projectId: Project['id'] }) => {
  const listQuery = useTagListQuery({ projectId });
  const saveManyMutation = useSaveManyTagsMutation({ projectId });

  const { onSearch, debouncedSearch, search } = useDebounceInput('');
  const [status, setStatus] = useState<boolean | null>(null);
  const [showForm, { setTrue: openForm, setFalse: closeForm }] = useBoolean();

  const filteredList = useDataFilter(
    listQuery.data || [],
    [
      (x) => status === null || x.enabled === status,
      (x) =>
        !debouncedSearch ||
        x.key.toLowerCase().includes(debouncedSearch.trim().toLowerCase()),
    ],
    [status, debouncedSearch.trim().toLowerCase()]
  );

  const saveHandler = useCallback(
    async (tags: string[]) => {
      await saveManyMutation.mutateAsync({ tags, enabled: false });
      closeForm();
    },
    [closeForm, saveManyMutation]
  );

  if (listQuery.isLoading) {
    return (
      <Row justify="center" align="middle" className="w--full h--full">
        <Spin />
      </Row>
    );
  }
  if (!listQuery.data) {
    return <Empty />;
  }
  return (
    <div className={cn(boxCss.flex, boxCss.flexColumn, boxCss.fullHeight)}>
      <Row justify="space-between" className={boxCss.flex00Auto}>
        <Space>
          <Input
            value={search}
            placeholder="Search"
            onChange={onSearch}
            suffix={<SearchOutlined />}
          />
          <OnOffSelector value={status} onChange={setStatus} />
        </Space>
        <Button onClick={openForm}>
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
        <Table.Column key="key" dataIndex="key" width="100%" />
        <Table.Column
          key="enabled"
          render={(tag: Tag) => (
            <TagActions key={tag.key} tag={tag} projectId={projectId} />
          )}
        />
      </Table>
      <Drawer
        visible={showForm}
        title="Add tags"
        onClose={closeForm}
        width="400px"
      >
        {showForm && (
          <TagCreateForm
            loading={saveManyMutation.isLoading}
            onSave={saveHandler}
            onCancel={closeForm}
            tags={listQuery.data}
          />
        )}
      </Drawer>
    </div>
  );
};
