import React from "react";
import { PermissionCheckerParams, useHasPermission$ } from "@core/auth";
import { observer } from "mobx-react-lite";

export const HasPermission: React.FC<
  {
    permissions: string[] | string;
    deniedView: React.ReactElement;
  } & PermissionCheckerParams
> = observer(
  ({ permissions, all, ignoreProject, children, deniedView: deniedView }) => {
    const hasPermissions = useHasPermission$(permissions, {
      all,
      ignoreProject,
    });
    return hasPermissions ? <>{children}</> : <>{deniedView}</>;
  }
);
