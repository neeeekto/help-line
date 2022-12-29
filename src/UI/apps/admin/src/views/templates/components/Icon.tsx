import React from 'react';
import {
  FileExclamationOutlined,
  FileTextOutlined,
  Html5Filled,
} from '@ant-design/icons';
import { ResourceType } from '../state';

export const Icon: React.FC<{ type: ResourceType }> = ({ type }) => {
  if (type === ResourceType.Context) return <FileExclamationOutlined />;
  if (type === ResourceType.Component) return <FileTextOutlined />;
  return <Html5Filled />;
};
