import React, { ChangeEvent, useCallback, useState } from 'react';
import { FullPageContainer } from '@help-line/components';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { HelpdeskAdminApi } from '@help-line/entities/admin/api';
import { Button, Input, message, Tooltip, Typography } from 'antd';
import { ReloadOutlined, SearchOutlined } from '@ant-design/icons';
import { DiffEditor } from '@monaco-editor/react';
import { useApi } from '@help-line/modules/api';
import { TicketId } from '@help-line/entities/client/api';

export const TicketIndex: React.FC = () => {
  const helpdeskApi = useApi(HelpdeskAdminApi);
  const [ticketId, setTicketId] = useState<TicketId>('');
  const [original, setOriginal] = useState<any[]>([]);
  const [diff, setDiff] = useState<[string, string]>(['', '']);

  const onInput = useCallback(
    (evt: ChangeEvent<HTMLInputElement>) => {
      setTicketId(evt.currentTarget.value as TicketId);
    },
    [setTicketId]
  );

  const onReindex = useCallback(async () => {
    try {
      const oldData = await helpdeskApi.getTicketView({ ticketId });
      await helpdeskApi.recreateTicketView({ ticketId });
      const newData = await helpdeskApi.getTicketView({ ticketId });
      setDiff([
        JSON.stringify(oldData, null, 2),
        JSON.stringify(newData, null, 2),
      ]);
      setOriginal([oldData, newData]);
      message.success('Ticket view has recreated');
    } catch (e: any) {
      message.error(e.response?.data?.detail);
    }
  }, [setDiff, ticketId]);
  return (
    <FullPageContainer
      className={cn(boxCss.flex, boxCss.flexColumn, spacingCss.spaceSm)}
    >
      <Typography.Title level={4}>Ticket view</Typography.Title>
      <div
        className={cn(boxCss.flex, boxCss.alignItemsCenter, spacingCss.spaceXs)}
      >
        <Input
          prefix="Ticket ID:"
          placeholder={'X-XXXXXX'}
          value={ticketId}
          onChange={onInput}
          size="small"
        />
        <Tooltip
          title="Reindex ticket view"
          mouseEnterDelay={1}
          placement={'topRight'}
        >
          <Button
            size="small"
            type="dashed"
            disabled={!ticketId}
            onClick={onReindex}
            icon={<ReloadOutlined />}
          />
        </Tooltip>
      </div>
      <FullPageContainer>
        <DiffEditor original={diff[0]} modified={diff[1]} language="json" />
      </FullPageContainer>
    </FullPageContainer>
  );
};
