import React, { useMemo } from 'react';
import {
  TagDescription,
  TagDescriptionIssue,
} from '@help-line/entities/client/api';
import { useBoolean } from 'ahooks';
import { LinkOutlined, MessageOutlined } from '@ant-design/icons';
import cn from 'classnames';
import { boxCss, spacingCss, textCss } from '@help-line/style-utils';
import { Button } from 'antd';

const TagDescriptionIssueRow = ({
  viewLanguage,
  issue,
}: {
  viewLanguage: string;
  issue: TagDescriptionIssue;
}) => {
  const content = issue.contents[viewLanguage];
  return (
    <div
      className={cn(boxCss.flex, boxCss.alignItemsCenter, spacingCss.spaceSm)}
    >
      {content.uri ? <LinkOutlined /> : <MessageOutlined />}
      <div className={textCss.truncate}>{content.text}</div>
    </div>
  );
};

export const TagDescriptionIssues: React.FC<{
  description: TagDescription;
  viewLanguage: string;
}> = ({ description, viewLanguage }) => {
  const [showMore, changeShowMore] = useBoolean();
  const LIMIT = 3;
  const showList = useMemo(() => {
    if (showMore) {
      return description.issues;
    } else {
      return description.issues.slice(0, LIMIT);
    }
  }, [showMore]);
  return (
    <div className={cn(boxCss.flex, boxCss.flexColumn, spacingCss.spaceXs)}>
      {showList.map((x, inx) => (
        <TagDescriptionIssueRow
          key={inx}
          issue={x}
          viewLanguage={viewLanguage}
        />
      ))}
      {description.issues.length > LIMIT && (
        <Button type="link" onClick={changeShowMore.toggle}>
          {showMore ? 'Show more...' : 'Hide...'}
        </Button>
      )}
    </div>
  );
};
