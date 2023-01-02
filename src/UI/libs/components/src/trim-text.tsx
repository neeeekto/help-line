import React, { PropsWithChildren } from 'react';
import cn from 'classnames';
import { textCss } from '@help-line/style-utils';

export const TrimText = ({
  children,
  maxWith,
}: PropsWithChildren<{ maxWith: string }>) => {
  return (
    <span className={cn(textCss.truncate)} style={{ maxWidth: maxWith }}>
      {children}
    </span>
  );
};
