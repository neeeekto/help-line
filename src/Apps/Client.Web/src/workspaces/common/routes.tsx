import React, { useCallback } from "react";
import { Redirect, Route, Switch, useHistory } from "react-router-dom";
import { usePathMaker } from "@core/router";
import { TicketView } from "./views/ticket.view";
import { TicketsView } from "./views/tickets.view";
import { UsersList } from "../../views/users";
import { RolesList } from "@views/roles";
import { useRouteMatch } from "react-router";
import { RouteTabs } from "@shared/components/route-tabs";
import { Tags, TagsDescriptions } from "@views/tags-managment";
import { Spin } from "antd";

const TicketsFilter = React.lazy(() => import("../../views/tickets-filters"));

export const HdRoutes: React.FC = () => {
  const make = usePathMaker();
  return (
    <Switch>
      <Route path={make("tickets", ":ticketId")}>
        <TicketView />
      </Route>
      <Route path={make("tickets")}>
        <TicketsView />
      </Route>

      <Route path={make("filters")}>
        <RouteTabs>
          <RouteTabs.Tab path={make("filters")} name="Filters">
            <React.Suspense fallback={<Spin />}>
              <TicketsFilter />
            </React.Suspense>
          </RouteTabs.Tab>
        </RouteTabs>
      </Route>
      <Route path={make("problems", "current")}>problems current</Route>
      <Route path={make("problems", "templates")}>problems Templates</Route>
      <Route path={make("reports", "reports1")}>reports1</Route>
      <Route path={make("automations", "macros")}>macros</Route>
      <Route path={make("automations", "autoreply")}>autoreply</Route>
      <Route path={make("settings", "reminders")}>reminders</Route>
      <Route path={make("settings", "message-templates")}>
        message-templates
      </Route>
      <Route path={make("settings", "reopen-conditions")}>
        reopen-conditions
      </Route>
      <Route path={make("settings", "bans")}>bans</Route>
      <RouteTabs>
        <RouteTabs.Tab path={make("settings", "tags")} name="Tags">
          <Tags />
        </RouteTabs.Tab>

        <RouteTabs.Tab
          path={make("settings", "tags-descriptions")}
          name="Descriptions"
        >
          <TagsDescriptions />
        </RouteTabs.Tab>
      </RouteTabs>
      <Route path={make("operators")}>
        <RouteTabs>
          <RouteTabs.Tab path={make("operators", "all")} name="Operators">
            <Tags />
          </RouteTabs.Tab>

          <RouteTabs.Tab path={make("operators", "roles")} name="Roles">
            <TagsDescriptions />
          </RouteTabs.Tab>
        </RouteTabs>
      </Route>
      <Route path={make("settings", "delays")}>delays</Route>
      <Redirect to={make("hd", "tickets")} />
    </Switch>
  );
};

export const UARoutes: React.FC = () => {
  const make = usePathMaker();
  return (
    <RouteTabs>
      <RouteTabs.Tab path={make("users")} name="Users">
        <UsersList />
      </RouteTabs.Tab>
      <RouteTabs.Tab path={make("roles")} name="Roles">
        <RolesList />
      </RouteTabs.Tab>
    </RouteTabs>
  );
};
