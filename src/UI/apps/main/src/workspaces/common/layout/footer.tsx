import React, { memo, useMemo } from 'react';
import { Dropdown, Menu, Avatar } from 'antd';
import cn from 'classnames';
import { Project } from '@help-line/entities/client/api';
import { boxCss, mouseCss, spacingCss, textCss } from '@help-line/style-utils';
import { useProjectsQueries } from '@help-line/entities/client/query';
import { ItemType } from 'antd/es/menu/hooks/useItems';
import { faQuestionCircle } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const DropdownMenu: React.FC<{
  projects: Project[];
  selected?: Project['id'];
  onSelect: (project: Project) => void;
}> = memo(({ projects, selected, onSelect }) => {
  return (
    <Menu>
      {projects.map((x) => (
        <Menu.Item
          key={x.id}
          disabled={selected === x.id}
          onClick={() => onSelect(x)}
        >
          <Avatar src={x.info.image} />
          <span className={cn(textCss.truncate, spacingCss.marginLeftSm)}>
            {x.info.name}
          </span>
        </Menu.Item>
      ))}
    </Menu>
  );
});

export const ProjectSelector = ({
  onSelectProject,
  projectId,
}: {
  projectId?: Project['id'];
  onSelectProject?: (project: Project) => void;
}) => {
  const projects = useProjectsQueries();
  const project = useMemo(
    () => projects.data?.find((x) => x.id === projectId),
    [projectId, projects.data]
  );
  const items = useMemo(
    () =>
      projects.data?.map(
        (x) =>
          ({
            key: x.id,
            icon: <Avatar src={x.info.image} />,
            label: (
              <span className={cn(textCss.truncate, spacingCss.marginLeftSm)}>
                {x.info.name}
              </span>
            ),
            onClick: () => onSelectProject?.(x),
          } as ItemType)
      ),
    [projects.data, onSelectProject]
  );

  return (
    <Dropdown
      placement="topRight"
      menu={{
        items,
        theme: 'light',
      }}
      trigger={['click']}
    >
      <div
        className={cn(
          spacingCss.paddingSm,
          textCss.white,
          boxCss.flex,
          boxCss.alignItemsCenter,
          mouseCss.pointer
        )}
      >
        <Avatar
          size="default"
          shape="circle"
          src={project?.info.image}
          icon={project ? null : <FontAwesomeIcon icon={faQuestionCircle} />}
        />
        <span
          className={cn(textCss.truncate, spacingCss.marginLeftSm)}
          style={{ color: 'white' }}
        >
          {project?.info.name || 'Unknown project'}
        </span>
      </div>
    </Dropdown>
  );
};
