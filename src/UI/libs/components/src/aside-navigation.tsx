import React, { PropsWithChildren, useMemo } from 'react';
import { Menu } from 'antd';
import { Link, useLocation } from 'react-router-dom';

export interface MenuItem<T = any> {
  segment: string;
  icon: React.ReactElement;
  custom?: React.ReactElement;
  content: React.ReactElement | string;
  data?: T;
}

export interface MenuGroup<T = any> extends MenuItem<T> {
  items: MenuElement<T>[];
}

export type MenuElement<T = any> = MenuItem<T> | MenuGroup<T>;

const isMenuGroup = (el: MenuElement): el is MenuGroup => {
  return !!(el as MenuGroup).items;
};

const AsideNavigationItem: React.FC<{
  element: MenuElement;
  parents: MenuGroup[];
}> = React.memo(({ element, parents }) => {
  const path = [...(parents ?? []), element];
  const key = path.map((x) => x.segment).join('/');
  const props = { eventKey: key }; //antd menu props

  if (isMenuGroup(element)) {
    return (
      <Menu.SubMenu
        key={key}
        title={element.content}
        icon={element.icon}
        {...props}
      >
        {element.items.map((x) => (
          <AsideNavigationItem element={x} parents={path as MenuGroup[]} />
        ))}
        {element.custom}
      </Menu.SubMenu>
    );
  }
  return (
    <Menu.Item key={key} icon={element.icon} {...props}>
      <Link to={key}>{element.content}</Link>
    </Menu.Item>
  );
});
export const AsideNavigation = ({
  menu,
  className,
  children,
}: PropsWithChildren<{ menu: MenuElement[]; className?: string }>) => {
  const location = useLocation();
  const segments = useMemo(
    () =>
      location.pathname
        .replace('#', '')
        .split('/')
        .filter((x) => !!x),
    [location.pathname]
  );
  const path = useMemo(() => {
    let prev = '';
    const result = [];
    for (const segment of segments) {
      prev = `${prev}/${segment}`;
      result.push(prev);
    }
    return result;
  }, [segments]);

  return (
    <Menu
      theme="dark"
      mode="inline"
      className={className}
      defaultSelectedKeys={path}
      selectedKeys={path}
      defaultOpenKeys={path}
    >
      {menu.map((x) => {
        return <AsideNavigationItem key={x.segment} element={x} parents={[]} />;
      })}
      {children}
    </Menu>
  );
};
