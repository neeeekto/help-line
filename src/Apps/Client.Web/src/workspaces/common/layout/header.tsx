import React from "react";
import { Button, Layout } from "antd";

export const Header: React.FC<{ onCollapse: () => void }> = ({
  onCollapse,
}) => {
  return (
    <Layout.Header>
      <Button size="small" onClick={onCollapse}>
        Collapse
      </Button>
    </Layout.Header>
  );
};
