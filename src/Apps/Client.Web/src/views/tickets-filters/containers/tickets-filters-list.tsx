import { observer } from "mobx-react-lite";
import { useAuthStore$ } from "@core/auth";
import { useSystemStore$ } from "@core/system";
import {
  TicketFilter,
  useDeleteTicketFilterMutation,
  useTicketsFiltersQuery,
} from "@entities/helpdesk/tickets";
import {
  Button,
  Input,
  Popconfirm,
  Row,
  Select,
  Skeleton,
  Table,
  Tag,
  Tooltip,
  Typography,
} from "antd";
import { useDebounceInput } from "@shared/hooks/input";
import React, { useMemo, useState } from "react";
import { useDataFilter } from "@shared/hooks/data-filter.hooks";
import cn from "classnames";
import { boxCss, mouseCss, spacingCss, textCss } from "@shared/styles";
import {
  CopyOutlined,
  DeleteOutlined,
  GlobalOutlined,
  SearchOutlined,
  UsergroupAddOutlined,
} from "@ant-design/icons";
import { QuerySimpleLoading } from "@shared/components/query-simple-loading";
import { USER_LAZY_ATTENTION_DELAY } from "@shared/constants";
import { TicketFilterFeatureTag } from "@views/tickets-filters/components/ticket-filter-feature-tag";
import {
  OperatorView,
  useOperatorsViewQuery,
} from "@entities/helpdesk/operators";
import { PropsWithClassName } from "@shared/react.types";
import { useCurrentProjectId$ } from "@core/system/system.context";

const TicketsFiltersListSkeleton: React.FC = () => {
  return (
    <div>
      <Row justify="space-between">
        <Skeleton.Input active />
      </Row>
      <Skeleton active />
    </div>
  );
};

const TicketFilterActions: React.FC<{ filter: TicketFilter }> = observer(
  ({ filter }) => {
    const projectId = useCurrentProjectId$()!;
    const deleteMutation = useDeleteTicketFilterMutation(projectId, filter.id);
    return (
      <>
        <Button type="text" size="small" icon={<CopyOutlined />} />
        <Popconfirm
          title="Do you sure you want remove this filter?"
          placement="topRight"
          okButtonProps={{ danger: true }}
          okText="Yes"
          onConfirm={deleteMutation.mutate as any}
        >
          <Button
            type="text"
            size="small"
            loading={deleteMutation.isLoading}
            icon={<DeleteOutlined />}
          />
        </Popconfirm>
      </>
    );
  }
);

const TicketFilterShare: React.FC<
  {
    filter: TicketFilter;
    operators?: OperatorView[];
  } & PropsWithClassName
> = React.memo(({ filter, operators }) => {
  if (!filter.share) {
    return null;
  }

  if (filter.share.$type === "TicketFilterShareGlobal") {
    return (
      <Tooltip
        title="Global filter, all operators can use it"
        mouseEnterDelay={USER_LAZY_ATTENTION_DELAY}
      >
        <Tag>
          <GlobalOutlined />
        </Tag>
      </Tooltip>
    );
  }

  return (
    <Tooltip
      title={
        <div>
          Share filter, operators from the list can use it: <br />
          <div className={boxCss.overflowAuto} style={{ maxHeight: 100 }}>
            {filter.share.operators
              .map((x) => {
                const oper = operators?.find((o) => x === o.id);
                return `${oper?.firstName} ${oper?.lastName}`;
              })
              .join(", ")}
          </div>
        </div>
      }
      mouseEnterDelay={USER_LAZY_ATTENTION_DELAY}
    >
      <Tag icon={<UsergroupAddOutlined />}>
        (+{filter.share.operators.length})
      </Tag>
    </Tooltip>
  );
});

const TicketFilterOwner: React.FC<{
  operators?: OperatorView[];
  filter: TicketFilter;
}> = observer(({ filter, operators }) => {
  const authStore = useAuthStore$();
  const operator = useMemo(() => {
    return operators?.find((x) => x.id === filter.owner);
  }, [filter.owner, operators]);
  if (authStore.state.me?.id === filter.owner) {
    return (
      <Typography.Text type="secondary" strong>
        You owner
      </Typography.Text>
    );
  }
  if (filter.owner) {
    return (
      <Typography.Text type="secondary">
        {operator?.firstName} {operator?.lastName}
      </Typography.Text>
    );
  }
  return null;
});

export const TicketsFiltersList = observer(() => {
  const authStore = useAuthStore$();
  const currentProjectId = useCurrentProjectId$()!;

  const filtersQuery = useTicketsFiltersQuery(currentProjectId);
  const operatorsQuery = useOperatorsViewQuery(currentProjectId);

  const [operatorFilter, setOperatorFilter] = useState<
    OperatorView["id"] | null | undefined
  >(authStore.state.me?.id);

  const { onSearch, debouncedSearch, search } = useDebounceInput("");

  const filteredList = useDataFilter(
    filtersQuery.data || [],
    [
      (x, value) => !value || x.name.toLowerCase().includes(value),
      (x, value) =>
        value === undefined || (value === null ? !x.owner : x.owner === value),
    ],
    [debouncedSearch.trim().toLowerCase(), operatorFilter]
  );

  return (
    <QuerySimpleLoading
      query={filtersQuery}
      loading={<TicketsFiltersListSkeleton />}
    >
      <div className={cn(boxCss.flex, boxCss.flexColumn, boxCss.fullHeight)}>
        <div
          className={cn(
            boxCss.flex00Auto,
            boxCss.flex,
            boxCss.justifyContentSpaceBetween,
            spacingCss.spaceMd
          )}
        >
          <div className={cn(boxCss.flex, spacingCss.spaceMd)}>
            <Input
              value={search}
              placeholder="Search"
              onChange={onSearch}
              suffix={<SearchOutlined />}
            />
            <Select
              value={operatorFilter}
              style={{ width: 300 }}
              allowClear
              placeholder="Owner"
              onChange={setOperatorFilter}
            >
              {!authStore.profile.get()?.isAdmin && (
                <Select.Option value={authStore.state?.me?.id}>
                  You owner
                </Select.Option>
              )}
              <Select.Option value={null}>System</Select.Option>

              {operatorsQuery.data
                ?.filter((x) => x.id !== authStore.state?.me?.id)
                .map((x) => (
                  <Select.Option key={x.id} value={x.id}>
                    {x.firstName} {x.lastName}
                  </Select.Option>
                ))}
            </Select>
          </div>
          <Button type="primary">+ Add</Button>
        </div>
        <Table
          dataSource={filteredList!}
          pagination={false}
          size="small"
          showHeader={false}
          className={cn(
            boxCss.fullHeight,
            boxCss.fullWidth,
            boxCss.overflowAuto,
            spacingCss.marginTopLg
          )}
        >
          <Table.Column
            key="name"
            width="50%"
            ellipsis
            render={(data: TicketFilter) => (
              <Tooltip
                title={data.name}
                mouseEnterDelay={USER_LAZY_ATTENTION_DELAY}
                className={textCss.truncate}
              >
                <div className={textCss.truncate}>
                  <Typography.Link>{data.name}</Typography.Link>
                </div>
              </Tooltip>
            )}
          />

          <Table.Column
            key="share"
            width="200px"
            render={(data: TicketFilter) => (
              <div className={cn(mouseCss.lowAttention)}>
                <TicketFilterShare
                  filter={data}
                  operators={operatorsQuery?.data}
                />
                {data.features.map((x) => (
                  <TicketFilterFeatureTag feat={x} key={x} />
                ))}
              </div>
            )}
          />

          <Table.Column
            key="owner"
            render={(data: TicketFilter) => (
              <TicketFilterOwner
                filter={data}
                operators={operatorsQuery.data}
              />
            )}
          />

          <Table.Column
            key="actions"
            render={(data: TicketFilter) => (
              <TicketFilterActions filter={data} />
            )}
          />
        </Table>
      </div>
    </QuerySimpleLoading>
  );
});
