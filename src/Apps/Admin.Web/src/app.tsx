import React from "react";
import { BrowserRouter, Switch, Route, Redirect } from "react-router-dom";
import { LayoutRoot } from "@shared/layout";
import { PageSpin } from "@shared/components/page-spin";
import { observer } from "mobx-react-lite";
import { MigrationsProvider } from "@views/migrations";

const Templates = React.lazy(() => import("./views/templates"));
const Jobs = React.lazy(() => import("@views/jobs"));
const Projects = React.lazy(() => import("./views/projects"));
const TicketTimers = React.lazy(() => import("./views/tickets-timers"));
const CreateTicket = React.lazy(() => import("./views/create-ticket"));
const TicketIndex = React.lazy(() => import("./views/ticket-index"));

export const App = observer(() => {
  return (
    <BrowserRouter>
      <MigrationsProvider>
        <LayoutRoot>
          <Switch>
            <Route path="/projects">
              <React.Suspense fallback={<PageSpin />}>
                <Projects />
              </React.Suspense>
            </Route>
            <Route path="/templates">
              <React.Suspense fallback={<PageSpin />}>
                <Templates />
              </React.Suspense>
            </Route>
            <Route path="/jobs">
              <React.Suspense fallback={<PageSpin />}>
                <Jobs />
              </React.Suspense>
            </Route>
            <Route path="/timers">
              <React.Suspense fallback={<PageSpin />}>
                <TicketTimers />
              </React.Suspense>
            </Route>
            <Route path="/create-ticket">
              <React.Suspense fallback={<PageSpin />}>
                <CreateTicket />
              </React.Suspense>
            </Route>
            <Route path="/ticket-index">
              <React.Suspense fallback={<PageSpin />}>
                <TicketIndex />
              </React.Suspense>
            </Route>
            <Route path="**">
              <Redirect to="timers" />
            </Route>
          </Switch>
        </LayoutRoot>
      </MigrationsProvider>
    </BrowserRouter>
  );
});
