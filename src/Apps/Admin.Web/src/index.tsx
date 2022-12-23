import React from "react";
import ReactDOM from "react-dom";
import { App } from "./app";
import reportWebVitals from "./reportWebVitals";

import "./styles";
import { QueryProvider } from "@core/queries";
import { AuthProvider } from "@core/auth";
import { HttpProvider } from "@core/http";
import { setupCore } from "@core/setup";
import { AuthGuard } from "@core/auth/components/auth-guard";

setupCore();

ReactDOM.render(
  <React.StrictMode>
    <QueryProvider>
      <AuthProvider>
        <HttpProvider>
          <AuthGuard>
            <App />
          </AuthGuard>
        </HttpProvider>
      </AuthProvider>
    </QueryProvider>
  </React.StrictMode>,
  document.getElementById("root")
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
