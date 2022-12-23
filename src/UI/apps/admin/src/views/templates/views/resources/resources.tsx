import React, { useMemo, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, Collapse, Input, Tree } from 'antd';
import {
  useContextsQuery,
  useTemplatesQuery,
  useTemplateSaveMutation,
  useComponentsQuery,
} from '@help-line/entities/admin/query';
import { Resource } from './resource';

import { SourceType } from '../../state/editro.types';

export const Resources: React.FC = observer(() => {
  const tmplQuery = useTemplatesQuery();

  return (
    <>
      <Resource
        src={SourceType.Template}
        name="Templates"
        lang="handlebars"
        queryList={tmplQuery}
      />
      {/*<Resource
        src={SourceType.Component}
        name="Components"
        lang="handlebars"
        queries={componentsQueries}
      />

      <Resource
        src={SourceType.Context}
        name="Contexts"
        lang="json"
        queries={contextQueries}
      />*/}
    </>
  );
});
