import React, { ChangeEvent, useCallback } from 'react';
import { FieldBuilderProps } from './data-builder.type';
import { PrimitiveFieldType, Primitives } from '@help-line/entities/share';
import { usePath } from './data-builder.hooks';
import { RadioChangeEvent } from 'antd/es';
import { Moment } from 'moment';
import { DatePicker, Input, InputNumber, Radio } from 'antd';
import { RowFieldView } from './row-field-view';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const PrimitiveFieldValue: React.FC<
  FieldBuilderProps & {
    type: PrimitiveFieldType;
  }
> = ({ field, description, parent, type, value, setValue }) => {
  const path = usePath(...(parent || []), field);
  const onChange = useCallback(
    (evt: any) => {
      const update = setValue ? setValue : (val: any, path: string[]) => {};
      switch (type.type) {
        case Primitives.Number:
          update(evt, path);
          break;
        case Primitives.Boolean:
          const radioEvt = evt as RadioChangeEvent;
          update(radioEvt.target.value === 'true', path);
          break;
        case Primitives.String:
          const inpEvent = evt as ChangeEvent<HTMLInputElement>;
          update(inpEvent.currentTarget.value, path);
          break;
        case Primitives.Date:
          const datePickEvent = evt as Moment;
          update(datePickEvent.toDate().toISOString(), path);
          break;
      }
    },
    [path, setValue]
  );
  switch (type.type) {
    case Primitives.Number:
      return <InputNumber size="small" value={value} onChange={onChange} />;
    case Primitives.Boolean:
      return (
        <Radio.Group size="small" value={value} onChange={onChange}>
          <Radio.Button value={true}>True</Radio.Button>
          <Radio.Button value={false}>False</Radio.Button>
        </Radio.Group>
      );
    case Primitives.String:
      return <Input size="small" value={value} onChange={onChange} />;
    case Primitives.Date:
      return (
        <DatePicker size="small" showTime value={value} onChange={onChange} />
      );
  }
};

export const PrimitiveField: React.FC<
  FieldBuilderProps & {
    type: PrimitiveFieldType;
  }
> = React.memo((props) => {
  return (
    <RowFieldView field={props.field} icon={<FontAwesomeIcon icon="hashtag" />}>
      <PrimitiveFieldValue {...props} />
    </RowFieldView>
  );
});
