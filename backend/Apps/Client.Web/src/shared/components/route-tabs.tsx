import React, { PropsWithChildren, ReactNode, useCallback } from "react";
import {
  Route,
  Switch,
  useHistory,
  useLocation,
  generatePath,
} from "react-router-dom";
import { Tabs } from "antd";
import { usePathMaker } from "@core/router";
import { useRouteMatch } from "react-router";
import cn from "classnames";
import { boxCss, spacingCss, utilsCss } from "@shared/styles";

type RouteTabParams = PropsWithChildren<{ path: string; name?: ReactNode }>;

export const RouteTab: React.FC<RouteTabParams> = () => <></>;

export interface RouteTabsInterface
  extends React.FC<{
    children:
      | Array<React.ReactElement<RouteTabParams>>
      | React.ReactElement<RouteTabParams>;
    asRoot?: boolean;
  }> {
  Tab: typeof RouteTab;
}

export const RouteTabs: RouteTabsInterface = ({ children, asRoot }) => {
  const make = usePathMaker();
  const math = useRouteMatch();
  const { pathname } = useLocation();
  const router = useHistory();

  const onSelectTab = useCallback(
    (key: string) => {
      router.push(generatePath(key, math.params));
    },
    [make, router, math]
  );
  const tabs = (Array.isArray(children) ? children : [children]) as Array<
    React.ReactElement<RouteTabParams>
  >;
  return (
    <div
      className={cn(
        boxCss.flex,
        boxCss.flexColumn,
        boxCss.fullWidth,
        boxCss.fullHeight
      )}
    >
      <Tabs
        className={boxCss.flex00Auto}
        type="card"
        onTabClick={onSelectTab}
        activeKey={
          tabs.find((x) => pathname === generatePath(x.props.path, math.params))
            ?.props.path
        }
      >
        {tabs.map((x) => (
          <Tabs.TabPane key={x.props.path} tab={x.props.name || x.props.path} />
        ))}
      </Tabs>
      <div
        className={cn(
          boxCss.fullHeight,
          boxCss.fullWidth,
          utilsCss.bgWhite,
          spacingCss.paddingSm,
          boxCss.overflowAuto
        )}
      >
        <Switch>
          {tabs.map((x) => (
            <Route key={x.props.path} path={x.props.path}>
              {x.props.children}
            </Route>
          ))}
        </Switch>
      </div>
    </div>
  );
};
RouteTabs.Tab = RouteTab;
