import React, {
  ChangeEvent,
  useCallback,
  useEffect,
  useMemo,
  useState,
} from "react";
import { useSystemStore$ } from "@core/system";
import {
  TagDescriptionItem,
  useTagsDescriptionQueries,
} from "@entities/helpdesk/tags";
import first from "lodash/first";
import { observer } from "mobx-react-lite";
import { useDebounce, usePersistFn } from "ahooks";
import {
  Button,
  Drawer,
  Empty,
  Input,
  Popconfirm,
  Radio,
  Row,
  Select,
  Space,
  Spin,
  Switch,
  Table,
} from "antd";
import { FullPageContainer } from "@shared/components/full-page-container";
import cn from "classnames";
import { boxCss, spacingCss } from "@shared/styles";
import { PlusOutlined, SearchOutlined } from "@ant-design/icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { TagDescriptionIssues } from "@views/tags-managment/components/tag-description-issues";
import { useSystemSettingsQuery } from "@core/system/system.queries";
import { RadioChangeEvent } from "antd/lib/radio/interface";
import { OnOffSelector } from "@shared/components/on-off-selector";
import { useDataFilter } from "@shared/hooks/data-filter.hooks";

const TagDescriptionActions: React.FC<{ description: TagDescriptionItem }> =
  observer(({ description }) => {
    const systemStore = useSystemStore$();
    const { useRemoveMutation, useSaveMutation } = useTagsDescriptionQueries(
      systemStore.state.currentProject!
    );
    const removeMutation = useRemoveMutation(description.tag);
    const saveMutation = useSaveMutation(description.tag);
    const onRemove = useCallback(
      () => removeMutation.mutate(description.tag),
      [description, removeMutation]
    );

    return (
      <Space wrap={false} align="center">
        <Switch
          size="small"
          checked={description.enabled}
          loading={saveMutation.isLoading}
          disabled={removeMutation.isLoading}
          onChange={() =>
            saveMutation.mutate({
              key: description.tag,
              data: { ...description, enabled: !description.enabled },
            })
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

export const TagsDescriptions: React.FC = observer(() => {
  const systemStore = useSystemStore$();
  const systemSettings = useSystemSettingsQuery();
  const { useListQuery } = useTagsDescriptionQueries(
    systemStore.state.currentProject!
  );
  const listQuery = useListQuery();

  const [search, setSearch] = useState("");
  const [status, setStatus] = useState<boolean | null>(null);
  const [language, setLanguage] = useState("");

  useEffect(() => {
    setLanguage(
      systemSettings.data?.defaultLanguage ||
        first(systemSettings.data?.languages) ||
        ""
    );
  }, [systemSettings.data]);
  const debouncedSearch = useDebounce(search, { wait: 500 });
  const onSearch = usePersistFn((event: ChangeEvent<HTMLInputElement>) =>
    setSearch(event.target.value)
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
          render={(desc: TagDescriptionItem) => (
            <TagDescriptionActions description={desc} />
          )}
        />
      </Table>
    </FullPageContainer>
  );
});
