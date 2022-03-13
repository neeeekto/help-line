import { observer } from "mobx-react-lite";
import { useAuthStore$ } from "@core/auth";
import { useSystemStore$ } from "@core/system";
import {
  TicketFilter,
  useTicketsFiltersQuery,
} from "@entities/helpdesk/tickets";
import {
  Button,
  Drawer,
  Empty,
  Input,
  Row,
  Space,
  Spin,
  Table,
  Tabs,
  Typography,
} from "antd";
import { useDebounceInput } from "@shared/hooks/input";
import React, { useState } from "react";
import { useDataFilter } from "@shared/hooks/data-filter.hooks";
import cn from "classnames";
import { boxCss, spacingCss } from "@shared/styles";
import { PlusOutlined, SearchOutlined } from "@ant-design/icons";
import { OnOffSelector } from "@shared/components/on-off-selector";

export const TicketsFiltersList = observer(() => {
  const authStore = useAuthStore$();
  const systemStore = useSystemStore$();

  const filtersQuery = useTicketsFiltersQuery(
    systemStore.state.currentProject!
  );

  const { onSearch, debouncedSearch, search } = useDebounceInput("");

  const filteredList = useDataFilter(
    filtersQuery.data || [],
    [(x, value) => !value || x.name.toLowerCase().includes(value)],
    [debouncedSearch.trim().toLowerCase()]
  );

  if (filtersQuery.isLoading && !filtersQuery.isFetched) {
    return (
      <Row justify="center" align="middle" className="w--full h--full">
        <Spin />
      </Row>
    );
  }

  if (!filtersQuery.data) {
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
        </Space>
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
        <Table.Column key="name" dataIndex="name" title="Name" width="100%" />
        <Table.Column
          key="enabled"
          render={(data: TicketFilter) => data.name}
        />
      </Table>
    </div>
  );
});
