import React from "react";
import "./wdyr";
import ReactDOM from "react-dom";
import { App } from "./app";
import reportWebVitals from "./reportWebVitals";

import "./styles";
import { QueryProvider } from "@core/queries";
import { AuthProvider } from "@core/auth";
import { HttpProvider } from "@core/http";
import { setupCore } from "@core/setup";
import { SystemGuard, SystemProvider } from "@core/system/components";
import { AuthGuard } from "@core/auth/components/auth-guard";
import { EventsProvider } from "@core/events";

setupCore();

ReactDOM.render(
  <React.StrictMode>
    <QueryProvider>
      <AuthProvider>
        <SystemProvider>
          <HttpProvider>
            <EventsProvider>
              <SystemGuard>
                <AuthGuard>
                  <App />
                </AuthGuard>
              </SystemGuard>
            </EventsProvider>
          </HttpProvider>
        </SystemProvider>
      </AuthProvider>
    </QueryProvider>
  </React.StrictMode>,
  document.getElementById("root")
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
