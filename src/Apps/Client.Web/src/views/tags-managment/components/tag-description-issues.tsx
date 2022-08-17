import React, { useCallback, useMemo } from "react";
import {
  TagDescriptionIssue,
  TagDescriptionItem,
} from "@entities/helpdesk/tags";
import { useBoolean } from "ahooks";
import { LinkOutlined, MessageOutlined } from "@ant-design/icons";
import cn from "classnames";
import { boxCss, spacingCss, textCss } from "@shared/styles";
import { Button } from "antd";

const TagDescriptionIssueRow: React.FC<{
  viewLanguage: string;
  issue: TagDescriptionIssue;
}> = ({ viewLanguage, issue }) => {
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
  description: TagDescriptionItem;
  viewLanguage: string;
}> = ({ description, viewLanguage }) => {
  const [showMore, changeShowMore] = useBoolean();
  const LIMIT = 3;
  const onToggle = useCallback(
    () => changeShowMore.toggle(!showMore),
    [showMore]
  );
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
        <Button type="link" onClick={onToggle}>
          {showMore ? "Show more..." : "Hide..."}
        </Button>
      )}
    </div>
  );
};
