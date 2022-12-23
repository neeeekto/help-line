import React from "react";
import { BrowserRouter, Switch, Route } from "react-router-dom";
import { CommonWorkspace } from "@workspaces/common";

export const App = () => {
  return (
    <BrowserRouter>
      <Switch>
        <Route path="/workspace">Test</Route>
        <Route path="/">
          <CommonWorkspace />
        </Route>
      </Switch>
    </BrowserRouter>
  );
};
