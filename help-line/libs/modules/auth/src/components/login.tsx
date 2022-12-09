import React, { useCallback } from 'react';
import { observer } from 'mobx-react-lite';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faLock } from '@fortawesome/free-solid-svg-icons';
import { Card } from 'antd';
import { Button } from 'antd';
import { LoginOutlined } from '@ant-design/icons';
import { useLoginAction } from '../store';

export const Login: React.FC = observer(() => {
  const loginAction = useLoginAction();

  const onLogin = useCallback(() => {
    loginAction.mutate();
  }, []);

  return (
    <Card
      title={<FontAwesomeIcon icon={faLock} />}
      size="small"
      style={{ width: 300 }}
      extra={
        <Button
          size="small"
          onClick={onLogin}
          type="primary"
          loading={loginAction.isLoading}
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
