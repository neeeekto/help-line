import React, { useEffect, useRef } from "react";
import {
  useAppStateQuery,
  useSystemSettingsQuery,
} from "@core/system/system.queries";
import { InitView } from "@core/system/components/init-view";
import { Card, Typography } from "antd";
import { ToolOutlined } from "@ant-design/icons";
import { systemApi } from "@core/system/system.api";
import { notification } from "antd";
import { messageLvlToAntLvl } from "@core/system/system.utils";
import { Message } from "@core/system/system.types";
import { TimeAgo } from "@shared/components/time-ago";
import { boxCss, spacingCss } from "@shared/styles";
import cn from "classnames";

const SystemMessageContent: React.FC<{ message: Message }> = ({ message }) => {
  return (
    <div>
      <div>{message.data.text}</div>
      {message.data.willHappenAt && (
        <Typography.Text
          className={cn(
            boxCss.flex,
            boxCss.alignItemsCenter,
            spacingCss.marginTopSm,
            spacingCss.spaceXs
          )}
          type="secondary"
        >
          <span>Time left:</span>
          <TimeAgo value={message.data.willHappenAt!} />
        </Typography.Text>
      )}
    </div>
  );
};

export const SystemGuard: React.FC = ({ children }) => {
  const appState = useAppStateQuery();
  const settings = useSystemSettingsQuery();
  const showedMessage = useRef<Record<string, boolean>>({});

  useEffect(() => {
    const getMessages = async () => {
      const messages = await systemApi.getMessages();
      for (let message of messages) {
        if (showedMessage.current[message.id]) {
          continue;
        }

        notification.open({
          key: message.id,
          type: messageLvlToAntLvl(message.data.lvl),
          message: "System Message",
          description: <SystemMessageContent message={message} />,
          duration: 0,
          placement: "bottomRight",
          onClose: () => {
            showedMessage.current[message.id] = true;
          },
        });
      }
    };
    getMessages();
    const interval = setInterval(getMessages, 1000 * 60 * 10);
    return () => clearInterval(interval);
  }, []);

  if (
    (appState.isLoading && !appState.isFetched) ||
    (settings.isLoading && !settings.isFetched)
  ) {
    return <InitView text="Check app state..." />;
  }

  if (appState.data?.blocked) {
    return (
      <InitView>
        <Card>
          The application is offline. Technical works are being carried out{" "}
          <ToolOutlined />
        </Card>
      </InitView>
    );
  }

  return <>{children}</>;
};
