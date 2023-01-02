import React, { memo, useMemo } from "react";
import { Dropdown, Menu, Avatar } from "antd";
import { observer } from "mobx-react-lite";
import cn from "classnames";
import {Project} from "@help-line/entities/client/api";
import {spacingCss, textCss} from "@help-line/style-utils";
import {useProjectsQueries} from "@help-line/entities/client/query";

const DropdownMenu: React.FC<{
  projects: Project[];
  selected?: Project["id"];
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

export const Footer: React.FC = observer(() => {
  const projects = useProjectsQueries();
  const systemStore = useSystemStore$();
  const selected = useMemo(
    () =>
      projects.data?.find((x) => x.id === systemStore?.state.currentProject),
    [projects, systemStore.state?.currentProject]
  );

  return (
    <Dropdown
      placement="topRight"
      overlay={
        <DropdownMenu
          projects={projects.data!}
          selected={systemStore.state.currentProject}
          onSelect={(project) => systemStore.setProject(project)}
        />
      }
      trigger={["click"]}
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
        <Avatar size="default" shape="circle" src={selected?.info.image} />
        <span className={cn(textCss.truncate, spacingCss.marginLeftSm)}>
          {selected?.info.name}
        </span>
      </div>
    </Dropdown>
  );
});
