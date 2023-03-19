import React, { useCallback } from 'react';
import { observer } from 'mobx-react-lite';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faLock } from '@fortawesome/free-solid-svg-icons';
import { Card } from 'antd';
import { Button } from 'antd';
import { LoginOutlined } from '@ant-design/icons';
import { useAuthStore$ } from '../hooks';
import { useBoolean } from 'ahooks';

export const Login: React.FC = observer(() => {
  const authStore$ = useAuthStore$();
  const [loading, loadingCtrl] = useBoolean();

  const onLogin = useCallback(async () => {
    try {
      loadingCtrl.setTrue();
      await authStore$.login();
    } finally {
      loadingCtrl.setFalse();
    }
  }, [authStore$.login, loadingCtrl]);

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
          loading={loading}
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
