import React, { useMemo } from 'react';
import { spacingCss } from '@help-line/style-utils';

import { FieldBuilder } from './field-builder';
import { BuilderProps } from './data-builder.type';

export const ObjectBuilder: React.FC<
  BuilderProps & {
    type: string;
  }
> = ({ description, parent, type, value, setValue }) => {
  const typeDesc = useMemo(() => {
    return description.types.find((x) => x.key === type)!;
  }, [type]);
  return (
    <div className={spacingCss.paddingLeftLg}>
      {typeDesc.fields.map((x) => (
        <FieldBuilder
          key={x.path.join('.')}
          description={description}
          field={x}
          parent={parent}
          value={value}
          setValue={setValue}
        />
      ))}
    </div>
  );
};
