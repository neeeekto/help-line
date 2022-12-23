import React from 'react';
import {
  AppstoreAddOutlined,
  FileExclamationOutlined,
  FileTextOutlined,
  Html5Filled,
} from '@ant-design/icons';
import { SourceType } from '../state/editro.types';

export const Icon: React.FC<{ type: SourceType; field?: string }> = ({
  type,
  field,
}) => {
  if (type === SourceType.Context) return <FileExclamationOutlined />;
  if (type === SourceType.Component) return <FileTextOutlined />;
  if (field === 'props') return <AppstoreAddOutlined />;
  return <Html5Filled />;
};
