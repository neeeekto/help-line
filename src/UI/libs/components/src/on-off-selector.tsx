import React, { useCallback } from 'react';
import { Radio } from 'antd';
import { RadioChangeEvent } from 'antd/lib/radio/interface';

export const OnOffSelector = ({
  value,
  onChange,
  className,
}: {
  value: boolean | null;
  onChange: (value: boolean | null) => void;
  className?: string;
}) => {
  const onChangeStatus = useCallback(
    (event: RadioChangeEvent) =>
      onChange(
        event.target.value === 'null' ? null : event.target.value === 'true'
      ),
    [onChange]
  );
  return (
    <Radio.Group
      defaultValue={`${value}`}
      onChange={onChangeStatus}
      buttonStyle="solid"
      className={className}
    >
      <Radio.Button value="null">All</Radio.Button>
      <Radio.Button value="true">On</Radio.Button>
      <Radio.Button value="false">Off</Radio.Button>
    </Radio.Group>
  );
};
