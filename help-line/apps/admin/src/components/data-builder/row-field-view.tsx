import React, { PropsWithChildren } from 'react';
import { FieldDescription } from '@help-line/entities/share';
import cn from 'classnames';
import { boxCss, spacingCss } from '@help-line/style-utils';
import { FieldName } from './field-name';

export const RowFieldView: React.FC<
  PropsWithChildren<{
    field: FieldDescription;
    icon?: React.ReactElement;
  }>
> = ({ field, children, icon }) => (
  <div
    className={cn(
      boxCss.flex,
      boxCss.alignItemsCenter,
      spacingCss.marginBottomMd
    )}
  >
    <FieldName field={field} icon={icon} />
    {children}
  </div>
);
