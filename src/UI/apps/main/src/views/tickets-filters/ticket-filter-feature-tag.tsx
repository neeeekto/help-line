import React from 'react';
import { Tag, Tooltip } from 'antd';
import { PresetColorType, PresetStatusColorType } from 'antd/lib/_util/colors';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faRobot,
  faExclamationCircle,
} from '@fortawesome/free-solid-svg-icons';
import { LiteralUnion } from 'antd/lib/_util/type';
import { TicketFilterFeatures } from '@help-line/entities/client/api';
import { OPEN_HELP_DELAY } from '@help-line/modules/application';

const FeatureDesc = {
  [TicketFilterFeatures.Automations]: {
    icon: faRobot,
    desc: 'Can use/using in macros',
    color: '' as LiteralUnion<PresetColorType | PresetStatusColorType, string>,
    text: '',
  },
  [TicketFilterFeatures.Important]: {
    icon: faExclamationCircle,
    desc: 'Mark as Important (RED color)',
    color: '' as LiteralUnion<PresetColorType | PresetStatusColorType, string>,
    text: '',
  },
};

export const TicketFilterFeatureTag = ({
  feat,
}: {
  feat: TicketFilterFeatures;
}) => {
  const desc = FeatureDesc[feat];
  return (
    <Tooltip
      title={desc.desc}
      placement="topLeft"
      mouseEnterDelay={OPEN_HELP_DELAY}
    >
      <Tag color={desc.color}>
        {desc.icon ? <FontAwesomeIcon size="sm" icon={desc.icon} /> : null}
        {desc.text}
      </Tag>
    </Tooltip>
  );
};
