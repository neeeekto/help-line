import React, { useCallback, useState } from "react";
import { observer } from "mobx-react-lite";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import Card from "antd/es/card";
import { useAuthStore$ } from "@core/auth";
import { Button } from "antd";
import { LoginOutlined } from "@ant-design/icons";

export const Login: React.FC = observer(() => {
  const authStore = useAuthStore$();

  return (
    <Card
      title={<FontAwesomeIcon icon="lock" />}
      size="small"
      style={{ width: 300 }}
      extra={
        <Button
          size="small"
          onClick={authStore.login}
          type="primary"
          icon={<LoginOutlined />}
        >
          Login
        </Button>
      }
    >
      <p>Please, login...</p>
    </Card>
  );
});
