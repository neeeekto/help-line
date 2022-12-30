import { PropsWithChildren, ReactElement, ReactNode } from 'react';
import { usePathMaker } from '@help-line/modules/router';
import { antdCustomCss, boxCss } from '@help-line/style-utils';
import cn from 'classnames';
import { FullPageContainer } from '../full-page-container';
import { Tabs, theme } from 'antd';
import { useLocation } from 'react-router';
import {
  resolvePath,
  useNavigate,
  useNavigation,
  useResolvedPath,
} from 'react-router-dom';

export type RouteTabParams = PropsWithChildren<{
  path: string;
  name: ReactNode;
}>;
const Tab = (params: RouteTabParams) => <></>;

export interface TabsPageProps {
  children: Array<ReactElement<RouteTabParams>> | ReactElement<RouteTabParams>;
}

interface TabsViewProps extends TabsPageProps {
  children: Array<ReactElement<RouteTabParams>>;
  activeKey: string;
  onChange: (path: string) => void;
}

const TabsView = (props: TabsViewProps) => {
  const {
    token: { colorBgContainer, padding },
  } = theme.useToken();

  return (
    <FullPageContainer>
      <Tabs
        type="card"
        className={antdCustomCss.fullPageTabs}
        activeKey={props.activeKey}
        onChange={props.onChange}
      >
        {props.children.map((tab) => (
          <Tabs.TabPane
            tab={tab.props.name}
            key={tab.props.path}
            className={cn(boxCss.fullHeight, boxCss.overflowAuto)}
            style={{ background: colorBgContainer, padding }}
          >
            {tab.props.children}
          </Tabs.TabPane>
        ))}
      </Tabs>
    </FullPageContainer>
  );
};

export const TabsPage = (props: TabsPageProps) => {
  const tabs = Array.isArray(props.children)
    ? props.children
    : [props.children];

  const location = useLocation();
  const nav = useNavigate();
  console.log(location);
  const {
    token: { colorBgContainer, padding },
  } = theme.useToken();

  return (
    <FullPageContainer>
      <Tabs
        type="card"
        className={antdCustomCss.fullPageTabs}
        onChange={(path) => nav(resolvePath({ ...location, pathname: path }))}
      >
        {tabs.map((tab) => (
          <Tabs.TabPane
            tab={tab.props.name}
            key={tab.props.path}
            className={cn(boxCss.fullHeight, boxCss.overflowAuto)}
            style={{ background: colorBgContainer, padding }}
          >
            {tab.props.children}
          </Tabs.TabPane>
        ))}
      </Tabs>
    </FullPageContainer>
  );
};

TabsPage.Tab = Tab;
