import React from 'react';
import { FieldDescription } from '@help-line/entities/share';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { Typography } from 'antd';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import cn from 'classnames';

export const FieldName: React.FC<{
  field: FieldDescription;
  icon?: React.ReactElement;
}> = ({ field, icon }) => (
  <Typography.Text
    className={cn(
      spacingCss.marginRightSm,
      boxCss.inlineFlex,
      boxCss.alignItemsCenter
    )}
  >
    {icon && (
      <Typography.Text
        type="secondary"
        className={spacingCss.marginRightSm}
        style={{ fontSize: '10px' }}
      >
        {icon}
      </Typography.Text>
    )}
    {field.name || field.path.join('.')}
  </Typography.Text>
);
