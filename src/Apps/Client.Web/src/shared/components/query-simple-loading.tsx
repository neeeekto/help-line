import React from "react";
import { UseQueryResult } from "react-query/types/react/types";
import { Button, Result, Row, Spin } from "antd";
import { boxCss, spacingCss } from "@shared/styles";
import cn from "classnames";

export const QuerySimpleLoading: React.FC<{
  query: UseQueryResult;
  loading?: React.ReactElement;
  error?: React.ReactElement;
}> = (props) => {
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

  return <>{props.children}</>;
};
