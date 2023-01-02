import React from 'react';
import { EventViewProp } from '../discussion.types';
import { TicketDiscussionCard } from '../ticket-discussion-card';
import css from '../discussion.module.scss';
import { Divider, Tag, Tooltip, Typography } from 'antd';

import { InitiatorAndTime } from '../../../components/initiator-and-time';
import cn from 'classnames';
import { boxCss, mouseCss, spacingCss } from '@help-line/style-utils';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  TicketCreatedEvent,
  TicketInitiatorUtils,
  UserIdType,
} from '@help-line/entities/client/api';

export const TicketCreatedEventView = ({
  event,
}: EventViewProp<TicketCreatedEvent>) => {
  return (
    <TicketDiscussionCard
      className={cn(
        css.createView,
        TicketInitiatorUtils.isOperatorInitiator(event.initiator) &&
          css.createdByOperator
      )}
      right={!TicketInitiatorUtils.isUserInitiator(event.initiator)}
    >
      <Typography.Paragraph ellipsis={{ rows: 10, expandable: true }}>
        {event.message?.text}
      </Typography.Paragraph>
      <div className={cn(spacingCss.marginBottomSm, mouseCss.lowAttention)}>
        {event.userIds.map((x) => (
          <Tag key={x.userId}>
            {x.userId}
            <Divider type="vertical" />
            {x.channel}
            <Divider type="vertical" />
            <div
              className={cn(
                boxCss.inlineFlex,
                spacingCss.spaceSm,
                boxCss.alignItemsCenter
              )}
            >
              {x.type === UserIdType.Main && (
                <Tooltip title="Used as MAIN" mouseEnterDelay={1}>
                  <FontAwesomeIcon icon="key" />
                </Tooltip>
              )}
              {x.useForDiscussion && (
                <Tooltip title="Used for discussion" mouseEnterDelay={1}>
                  <FontAwesomeIcon icon="paper-plane" />
                </Tooltip>
              )}
            </div>
          </Tag>
        ))}
      </div>
      <div className={cn(css.info)}>
        <div>
          <Tooltip title="Source" mouseEnterDelay={1}>
            <span>{event.meta.source}</span>
          </Tooltip>
          <Divider type="vertical" />
          <span>{event.language.toUpperCase()}</span>
        </div>
        <InitiatorAndTime className={spacingCss.marginLeftAuto} evt={event} />
      </div>
    </TicketDiscussionCard>
  );
};
