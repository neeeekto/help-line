import React, { ChangeEvent, useCallback, useState } from 'react';
import { FullPageContainer } from '@help-line/components';
import cn from 'classnames';
import { boxCss, spacingCss, utilsCss } from '@help-line/style-utils';
import { helpdeskApi } from '@entities/helpdesk';
import { Button, Input, message } from 'antd';
import { ReloadOutlined, SearchOutlined } from '@ant-design/icons';
import { DiffEditor } from '@monaco-editor/react';
import { AxiosError } from 'axios';

const TicketIndex: React.FC = () => {
  const [ticketId, setTicketId] = useState('');
  const [original, setOriginal] = useState<any[]>([]);
  const [diff, setDiff] = useState<[string, string]>(['', '']);

  const onInput = useCallback(
    (evt: ChangeEvent<HTMLInputElement>) => {
      setTicketId(evt.currentTarget.value);
    },
    [setTicketId]
  );

  const onReindex = useCallback(async () => {
    try {
      const oldData = await helpdeskApi.getTicketView(ticketId);

      await helpdeskApi.recreateTicketView(ticketId);
      const newData = await helpdeskApi.getTicketView(ticketId);
      setDiff([
        JSON.stringify(oldData, null, 2),
        JSON.stringify(newData, null, 2),
      ]);
      setOriginal([oldData, newData]);
      message.success('Ticket view has recreated');
    } catch (e: any) {
      message.error(e.response?.data?.detail);
    }
  }, [setDiff, ticketId, setOriginal]);
  return (
    <FullPageContainer
      className={cn(boxCss.flex, boxCss.flexColumn, spacingCss.spaceMd)}
    >
      <div
        className={cn(
          utilsCss.bgWhite,
          spacingCss.paddingLg,
          boxCss.flex,
          boxCss.alignItemsCenter,
          spacingCss.spaceMd
        )}
      >
        <Input
          prefix="Ticket ID: "
          value={ticketId}
          onChange={onInput}
          size="small"
        />
        <Button
          size="small"
          type="dashed"
          disabled={!ticketId}
          onClick={onReindex}
        >
          <ReloadOutlined />
        </Button>
      </div>
      <FullPageContainer className={cn()}>
        <DiffEditor original={diff[0]} modified={diff[1]} language="json" />
      </FullPageContainer>
    </FullPageContainer>
  );
};

export default TicketIndex;
