import React from 'react';
import { observer } from 'mobx-react-lite';
import { Resource } from './resource';

import { ResourceType } from '../../state';

export const Resources: React.FC = observer(() => {
  return (
    <>
      <Resource
        type={ResourceType.Template}
        name="Templates"
        lang="handlebars"
      />
      <Resource
        type={ResourceType.Component}
        name="Components"
        lang="handlebars"
      />

      <Resource type={ResourceType.Context} name="Contexts" lang="json" />
    </>
  );
});
