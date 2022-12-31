import React, { useCallback, useState } from 'react';
import {
  useToggleProjectMutation,
  useProjectsQuery,
} from '@help-line/entities/admin/query';
import { FullPageContainer } from '@help-line/components';
import {
  Spin,
  List,
  Avatar,
  Switch,
  Tag,
  Button,
  Drawer,
  Typography,
} from 'antd';
import css from './projects.module.scss';
import cn from 'classnames';
import { useBoolean } from 'ahooks';
import { ProjectForm } from './project-form';
import { boxCss, mouseCss, spacingCss } from '@help-line/style-utils';
import { Project } from '@help-line/entities/admin/api';

const SwitchProject: React.FC<{ project: Project }> = ({ project }) => {
  const toggleProjectActivation = useToggleProjectMutation(project.id);
  const onChange = useCallback(
    () => toggleProjectActivation.mutate(project.active),
    [toggleProjectActivation, project.active]
  );

  return (
    <Switch
      checked={project.active}
      size="small"
      onChange={onChange}
      loading={toggleProjectActivation.isLoading}
    />
  );
};

export const Projects: React.FC = () => {
  const projectsQuery = useProjectsQuery();

  const [formIsShowed, { setTrue: showForm, setFalse: hideForm }] =
    useBoolean(false);
  const [editing, setEditing] = useState<Project | undefined>();
  const toEdit = useCallback(
    (item?: Project) => {
      showForm();
      setEditing(item);
    },
    [showForm, setEditing]
  );
  const add = useCallback(() => toEdit(), [toEdit]);

  if (projectsQuery.isLoading) {
    return (
      <FullPageContainer
        className={cn(
          boxCss.flex,
          boxCss.alignItemsCenter,
          boxCss.justifyContentCenter,
          boxCss.overflowAuto
        )}
      >
        <Spin />
      </FullPageContainer>
    );
  }

  return (
    <FullPageContainer
      className={cn(boxCss.flex, boxCss.overflowAuto, boxCss.flexColumn)}
    >
      <div
        className={cn(
          boxCss.flex,
          boxCss.justifyContentSpaceBetween,
          spacingCss.marginBottomSm
        )}
      >
        <Typography.Title level={4}>Projects</Typography.Title>

        <Button type="primary" onClick={add}>
          Add
        </Button>
      </div>
      <List
        itemLayout="horizontal"
        className={cn(css.list, boxCss.fullWidth)}
        dataSource={projectsQuery.data!}
        renderItem={(item) => (
          <List.Item
            className={spacingCss.paddingHorizSm}
            actions={[<SwitchProject project={item} />]}
          >
            <div
              className={cn(
                boxCss.flex,
                boxCss.alignItemsCenter,
                mouseCss.pointer,
                spacingCss.marginLeftSm
              )}
              onClick={() => toEdit(item)}
            >
              <Avatar src={item.info.image} />
              <span className={spacingCss.marginLeftSm}>{item.info.name}</span>
            </div>

            <div className={spacingCss.marginLeftAuto}>
              {item.languages.map((x) => (
                <Tag key={x} color="blue">
                  {x.toUpperCase()}
                </Tag>
              ))}
            </div>
          </List.Item>
        )}
      />
      <Drawer visible={formIsShowed} onClose={hideForm} width="500px">
        {formIsShowed && <ProjectForm project={editing} onClose={hideForm} />}
      </Drawer>
    </FullPageContainer>
  );
};
