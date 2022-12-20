import React, { memo, useMemo } from "react";
import { useLocation, Link } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useSystemStore$ } from "@core/system";
import {
  AsideNavigation,
  MenuElement,
} from "@shared/components/aside-navigation";
import { PermissionCheckerParams } from "@core/auth";
import { useProjectsQueries } from "@entities/helpdesk/projects";

interface ItemData {
  gameDeps: boolean;
  permissions: string | string[];
  permissionsCheckParams?: PermissionCheckerParams;
}

export const NavigationMenu = observer<
  React.PropsWithChildren<{ className?: string }>
>(({ className }) => {
  const projectsQuery = useProjectsQueries();
  const systemStore = useSystemStore$();
  const location = useLocation();
  const segments = useMemo(
    () =>
      location.pathname
        .replace("#", "")
        .split("/")
        .filter((x) => !!x),
    [location.pathname]
  );

  const menu = useMemo(
    () =>
      (
        [
          {
            segment: `/${systemStore.state.currentProject}/hd`,
            icon: <FontAwesomeIcon icon="comments" />,
            content: "Helpdesk",
            items: [
              {
                segment: "tickets",
                icon: <FontAwesomeIcon icon="ticket-alt" />,
                content: "Tickets",
              },
              {
                segment: "filters",
                icon: <FontAwesomeIcon icon="filter" />,
                content: "Filters",
              },
              {
                segment: `problems`,
                icon: <FontAwesomeIcon icon="business-time" />,
                content: "Problems",
                items: [
                  {
                    segment: "current",
                    icon: <FontAwesomeIcon icon="calendar-alt" />,
                    content: "Current",
                  },
                  {
                    segment: "templates",
                    icon: <FontAwesomeIcon icon="copy" />,
                    content: "Templates",
                  },
                ],
              },
              {
                segment: `automations`,
                icon: <FontAwesomeIcon icon="robot" />,
                content: "Automations",
                items: [
                  {
                    segment: "macros",
                    icon: <FontAwesomeIcon icon="code" />,
                    content: "Macros",
                  },
                  {
                    segment: "actions",
                    icon: <FontAwesomeIcon icon="terminal" />,
                    content: "Actions",
                  },
                  {
                    segment: "autoreply",
                    icon: <FontAwesomeIcon icon="reply-all" />,
                    content: "Autoreply",
                  },
                ],
              },
              {
                segment: `settings`,
                icon: <FontAwesomeIcon icon="cogs" />,
                content: "Settings",
                items: [
                  {
                    segment: "reminders",
                    icon: <FontAwesomeIcon icon="bell" />,
                    content: "Reminders",
                  },
                  {
                    segment: "message-templates",
                    icon: <FontAwesomeIcon icon="scroll" />,
                    content: "Message Templates",
                  },
                  {
                    segment: "reopen-conditions",
                    icon: <FontAwesomeIcon icon="history" />,
                    content: "Reopen Conditions",
                  },

                  {
                    segment: "bans",
                    icon: <FontAwesomeIcon icon="user-slash" />,
                    content: "Bans",
                  },
                  {
                    segment: "tags",
                    icon: <FontAwesomeIcon icon="tags" />,
                    content: "Tags",
                  },
                  {
                    segment: "tags-descriptions",
                    icon: <FontAwesomeIcon icon="tag" />,
                    content: "Tags Descriptions",
                  },
                  {
                    segment: "delays",
                    icon: <FontAwesomeIcon icon="hourglass-half" />,
                    content: "Delays",
                  },
                ],
              },
              {
                segment: `other`,
                content: "Other",
                icon: <FontAwesomeIcon icon="toolbox" />,
                items: [
                  {
                    segment: "unsubscribed",
                    content: "Unsubscribed",
                    icon: <FontAwesomeIcon icon="comment-slash" />,
                  },
                  {
                    segment: "creation-options",
                    icon: <FontAwesomeIcon icon="hammer" />,
                    content: "Creation Options",
                  },
                ],
              },
              {
                segment: `operators`,
                content: "Operators",
                icon: <FontAwesomeIcon icon="user-friends" />,
                items: [
                  {
                    segment: `all`,
                    content: "List",
                    icon: <FontAwesomeIcon icon="list" />,
                  },
                  {
                    segment: `roles`,
                    content: "Roles",
                    icon: <FontAwesomeIcon icon="user-tag" />,
                  },
                ],
              },
            ],
            data: {
              gameDeps: true,
            },
          },
          {
            segment: `/users-access`,
            icon: <FontAwesomeIcon icon="user-shield" />,
            content: "User Access",
            items: [
              {
                segment: "users",
                icon: <FontAwesomeIcon icon="users" />,
                content: "Users",
              },
              {
                segment: "roles",
                icon: <FontAwesomeIcon icon="id-card" />,
                content: "Roles",
              },
            ],
          },
        ] as MenuElement<ItemData>[]
      ).filter((x) => (projectsQuery.data?.length ? true : !x.data?.gameDeps)),
    [systemStore.state.currentProject, projectsQuery.data]
  );

  return <AsideNavigation menu={menu} className={className} />;
});
