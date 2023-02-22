import React, { ReactNode, useCallback } from 'react';
import { UseQueryResult } from '@tanstack/react-query';
import { Button, Result, Row, Spin } from 'antd';
import { boxCss } from '@help-line/style-utils';
import cn from 'classnames';

interface Props {
  query: UseQueryResult;
  loading?: React.ReactElement;
  error?: React.ReactElement;
  children?: ReactNode | undefined | (() => ReactNode);
}

export const QuerySimpleLoading = (props: Props) => {
  if (props.query.isLoading && !props.query.isFetched) {
    if (props.loading) {
      return <>{props.loading}</>;
    } else {
      return (
        <Row
          justify="center"
          align="middle"
          className={cn(boxCss.fullHeight, boxCss.fullWidth)}
        >
          <Spin size="large" />
        </Row>
      );
    }
  }
  if (props.query.isError) {
    if (props.error) {
      return <>{props.error}</>;
    } else {
      return (
        <Result
          status="error"
          title="Error"
          subTitle={`${props.query.error}`}
          extra={<Button onClick={() => props.query.refetch()}>Retry</Button>}
        />
      );
    }
  }
  if (typeof props.children === 'function') {
    return <>{props.children()}</>;
  }
  return <>{props.children || null}</>;
};
