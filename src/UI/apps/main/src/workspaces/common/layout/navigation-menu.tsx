import React, { useMemo } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faComments,
  faTicketAlt,
  faFilter,
  faBusinessTime,
  faCalendarAlt,
  faCopy,
  faRobot,
  faCode,
  faTerminal,
  faReplyAll,
  faCogs,
  faBell,
  faScroll,
  faHistory,
  faUserSlash,
  faTags,
  faTag,
  faHourglassHalf,
  faToolbox,
  faCommentSlash,
  faHammer,
  faUserFriends,
  faList,
  faUserTag,
  faUsers,
  faUserShield,
  faIdCard,
} from '@fortawesome/free-solid-svg-icons';

import { Project } from '@help-line/entities/client/api';
import { AsideNavigation, MenuElement } from '@help-line/components';
import { PermissionCheckerParams } from '@help-line/modules/auth';

interface ItemData {
  needProject: boolean;
  permissions: string | string[];
  permissionsCheckParams?: PermissionCheckerParams;
}

export const NavigationMenu = ({
  className,
  projectId,
  hideDependsByProject,
}: {
  className?: string;
  projectId?: Project['id'];
  hideDependsByProject?: boolean;
}) => {
  const menu = useMemo(() => {
    const onlyCommon = hideDependsByProject || !projectId;
    return (
      [
        {
          segment: `/${projectId}/hd`,
          icon: <FontAwesomeIcon icon={faComments} />,
          content: 'Helpdesk',
          items: [
            {
              segment: 'tickets',
              icon: <FontAwesomeIcon icon={faTicketAlt} />,
              content: 'Tickets',
            },
            {
              segment: 'filters',
              icon: <FontAwesomeIcon icon={faFilter} />,
              content: 'Filters',
            },
            {
              segment: `problems`,
              icon: <FontAwesomeIcon icon={faBusinessTime} />,
              content: 'Problems',
              items: [
                {
                  segment: 'current',
                  icon: <FontAwesomeIcon icon={faCalendarAlt} />,
                  content: 'Current',
                },
                {
                  segment: 'templates',
                  icon: <FontAwesomeIcon icon={faCopy} />,
                  content: 'Templates',
                },
              ],
            },
            {
              segment: `automations`,
              icon: <FontAwesomeIcon icon={faRobot} />,
              content: 'Automations',
              items: [
                {
                  segment: 'macros',
                  icon: <FontAwesomeIcon icon={faCode} />,
                  content: 'Macros',
                },
                {
                  segment: 'actions',
                  icon: <FontAwesomeIcon icon={faTerminal} />,
                  content: 'Actions',
                },
                {
                  segment: 'autoreply',
                  icon: <FontAwesomeIcon icon={faReplyAll} />,
                  content: 'Autoreply',
                },
              ],
            },
            {
              segment: `settings`,
              icon: <FontAwesomeIcon icon={faCogs} />,
              content: 'Settings',
              items: [
                {
                  segment: 'reminders',
                  icon: <FontAwesomeIcon icon={faBell} />,
                  content: 'Reminders',
                },
                {
                  segment: 'message-templates',
                  icon: <FontAwesomeIcon icon={faScroll} />,
                  content: 'Message Templates',
                },
                {
                  segment: 'reopen-conditions',
                  icon: <FontAwesomeIcon icon={faHistory} />,
                  content: 'Reopen Conditions',
                },

                {
                  segment: 'bans',
                  icon: <FontAwesomeIcon icon={faUserSlash} />,
                  content: 'Bans',
                },
                {
                  segment: 'tags',
                  icon: <FontAwesomeIcon icon={faTags} />,
                  content: 'Tags',
                },
                {
                  segment: 'tags-descriptions',
                  icon: <FontAwesomeIcon icon={faTag} />,
                  content: 'Tags Descriptions',
                },
                {
                  segment: 'delays',
                  icon: <FontAwesomeIcon icon={faHourglassHalf} />,
                  content: 'Delays',
                },
              ],
            },
            {
              segment: `other`,
              content: 'Other',
              icon: <FontAwesomeIcon icon={faToolbox} />,
              items: [
                {
                  segment: 'unsubscribed',
                  content: 'Unsubscribed',
                  icon: <FontAwesomeIcon icon={faCommentSlash} />,
                },
                {
                  segment: 'creation-options',
                  icon: <FontAwesomeIcon icon={faHammer} />,
                  content: 'Creation Options',
                },
              ],
            },
            {
              segment: `operators`,
              content: 'Operators',
              icon: <FontAwesomeIcon icon={faUserFriends} />,
              items: [
                {
                  segment: `all`,
                  content: 'List',
                  icon: <FontAwesomeIcon icon={faList} />,
                },
                {
                  segment: `roles`,
                  content: 'Roles',
                  icon: <FontAwesomeIcon icon={faUserTag} />,
                },
              ],
            },
          ],
          data: {
            needProject: true,
          },
        },
        {
          segment: `/users-access`,
          icon: <FontAwesomeIcon icon={faUserShield} />,
          content: 'User Access',
          items: [
            {
              segment: 'users',
              icon: <FontAwesomeIcon icon={faUsers} />,
              content: 'Users',
            },
            {
              segment: 'roles',
              icon: <FontAwesomeIcon icon={faIdCard} />,
              content: 'Roles',
            },
          ],
        },
      ] as MenuElement<ItemData>[]
    ).filter((x) => (onlyCommon ? !x.data?.needProject : true));
  }, [projectId, hideDependsByProject]);

  return <AsideNavigation menu={menu} className={className} />;
};
