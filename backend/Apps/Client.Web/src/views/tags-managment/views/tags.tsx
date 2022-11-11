import React, { ChangeEvent, useCallback, useMemo, useState } from "react";
import { useTagsQueries, Tag } from "@entities/helpdesk/tags";
import {
  Spin,
  Table,
  Row,
  Input,
  Space,
  Radio,
  Button,
  Switch,
  Popconfirm,
  Empty,
  Popover,
  Drawer,
} from "antd";
import { useBoolean } from "ahooks";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PlusOutlined, SearchOutlined } from "@ant-design/icons";
import { TagForm } from "../components/tag-form";
import cn from "classnames";
import { boxCss, spacingCss } from "@shared/styles";
import { observer } from "mobx-react-lite";
import { useSystemStore$ } from "@core/system";
import { OnOffSelector } from "@shared/components/on-off-selector";
import { useDebounceInput } from "@shared/hooks/input";
import { useDataFilter } from "@shared/hooks/data-filter.hooks";

const TagActions: React.FC<{ tag: Tag }> = observer(({ tag }) => {
  const systemStore = useSystemStore$();
  const { useRemoveMutation, useSaveMutation } = useTagsQueries(
    systemStore.state.currentProject!
  );
  const removeMutation = useRemoveMutation(tag.key);
  const saveMutation = useSaveMutation(tag.key);
  const onRemove = useCallback(
    () => removeMutation.mutate(tag.key),
    [tag, removeMutation]
  );

  return (
    <Space wrap={false} align="center">
      <Switch
        size="small"
        checked={tag.enabled}
        loading={saveMutation.isLoading}
        disabled={removeMutation.isLoading}
        onChange={() =>
          saveMutation.mutate({ key: tag.key, data: !tag.enabled })
        }
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
});

export const Tags: React.FC = observer(() => {
  const systemStore = useSystemStore$();
  const { useListQuery, useSaveManyMutation } = useTagsQueries(
    systemStore.state.currentProject!
  );
  const listQuery = useListQuery();
  const saveManyMutation = useSaveManyMutation();

  const { onSearch, debouncedSearch, search } = useDebounceInput("");
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
          render={(tag: Tag) => <TagActions tag={tag} />}
        />
      </Table>
      <Drawer
        visible={showForm}
        title="Add tags"
        onClose={closeForm}
        width="400px"
      >
        {showForm && (
          <TagForm
            loading={saveManyMutation.isLoading}
            onSave={saveHandler}
            onCancel={closeForm}
            tags={listQuery.data}
          />
        )}
      </Drawer>
    </div>
  );
});
