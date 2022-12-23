import { Menu } from 'antd';
import React, { memo, useMemo } from 'react';
import { useLocation, Link } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faCopy,
  faLayerGroup,
  faBusinessTime,
  faClock,
  faIndent,
  faPlus,
} from '@fortawesome/free-solid-svg-icons';

const Navigation: React.FC<{
  className?: string;
  segments: string[];
  collapse?: boolean;
}> = memo(({ className, segments, collapse }) => {
  return (
    <Menu
      theme="dark"
      mode="inline"
      inlineCollapsed={collapse}
      className={className}
      defaultSelectedKeys={segments}
      defaultOpenKeys={segments}
    >
      <Menu.Item key="projects" icon={<FontAwesomeIcon icon={faLayerGroup} />}>
        <Link to="/projects">Projects</Link>
      </Menu.Item>
      <Menu.Item key="templates" icon={<FontAwesomeIcon icon={faCopy} />}>
        <Link to="/templates">Templates</Link>
      </Menu.Item>
      <Menu.Item key="jobs" icon={<FontAwesomeIcon icon={faBusinessTime} />}>
        <Link to="/jobs">Jobs</Link>
      </Menu.Item>
      <Menu.Item key="timers" icon={<FontAwesomeIcon icon={faClock} />}>
        <Link to="/timers">Timers</Link>
      </Menu.Item>
      <Menu.Item key="ticket-index" icon={<FontAwesomeIcon icon={faIndent} />}>
        <Link to="/ticket-index">Indexes</Link>
      </Menu.Item>
      <Menu.Item key="create-ticket" icon={<FontAwesomeIcon icon={faPlus} />}>
        <Link to="/create-ticket">Create ticket</Link>
      </Menu.Item>
    </Menu>
  );
});

export const NavigationMenu = observer<
  React.PropsWithChildren<{ className?: string }>
>(({ className }) => {
  const location = useLocation();
  const segments = useMemo(
    () => location.pathname.split('/').filter((x) => !!x),
    [location.pathname]
  );
  return <Navigation className={className} segments={segments} />;
});
