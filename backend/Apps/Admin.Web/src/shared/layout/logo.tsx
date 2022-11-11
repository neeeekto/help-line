import React from "react";
import css from "./layout.module.scss";
import { useAuthStore$ } from "@core/auth";

export const Logo: React.FC<{ collapsed?: boolean }> = ({ collapsed }) => {
  return (
    <div className={css.logo}>
      <h3 className={css.logoText}>{collapsed ? "HL" : "Help Line Admin"}</h3>
    </div>
  );
};
