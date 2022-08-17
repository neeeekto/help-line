import React from "react";
import { Layout } from "antd";
import cl from "./layout.module.scss";

export const Content: React.FC = ({ children }) => {
  return <Layout.Content className={cl.content}>{children}</Layout.Content>;
};
