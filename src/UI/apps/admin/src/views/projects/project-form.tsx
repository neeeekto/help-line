import React, { useCallback, useMemo } from 'react';
import {
  useCreateProjectMutation,
  useUpdateProjectMutation,
} from '@help-line/entities/admin/query';
import { Button, Form, Input, Select } from 'antd';
import { CreateProjectData, Project } from '@help-line/entities/admin/api';

export const ProjectForm: React.FC<{
  project?: Project;
  onClose?: () => void;
}> = ({ project, onClose }) => {
  const updateProject = useUpdateProjectMutation(project?.id || '');
  const createProject = useCreateProjectMutation();

  const onApply = useCallback(
    async (val: CreateProjectData) => {
      if (project) {
        await updateProject.mutateAsync(val);
      } else {
        await createProject.mutateAsync(val);
      }
      onClose && onClose();
    },
    [createProject, onClose, project, updateProject]
  );

  const initValue = useMemo<CreateProjectData>(() => {
    return {
      name: project?.info.name ?? '',
      image: project?.info.image ?? '',
      languages: project?.languages ?? [],
      projectId: project?.id,
    } as CreateProjectData;
  }, [project]);
  return (
    <Form
      name="project"
      initialValues={initValue}
      layout="vertical"
      onFinish={onApply}
    >
      <Form.Item
        label="ID"
        name="projectId"
        rules={[{ required: true, message: 'Please input project name!' }]}
      >
        <Input disabled={!!project} />
      </Form.Item>
      <Form.Item
        label="Name"
        name="name"
        rules={[{ required: true, message: 'Please input project name!' }]}
      >
        <Input />
      </Form.Item>

      <Form.Item label="Image" name="image" rules={[]}>
        <Input />
      </Form.Item>

      <Form.Item
        label="Languages"
        name="languages"
        rules={[{ required: true, message: 'Languages is required!' }]}
      >
        <Select
          mode="tags"
          style={{ width: '100%' }}
          placeholder="Select languages"
        >
          {project?.languages.map((x) => (
            <Select.Option key={x} value={x}>
              {x}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>

      <Form.Item>
        <Button
          type="primary"
          htmlType="submit"
          loading={updateProject.isLoading || createProject.isLoading}
        >
          Submit
        </Button>
      </Form.Item>
    </Form>
  );
};
