import React, { useMemo, useState } from "react";
import { observer } from "mobx-react-lite";
import { Button, Collapse, Input, Tree } from "antd";
import {
  useComponentQueries,
  useContextQueries,
  useTemplatesQueries,
  TemplateItemQueries,
} from "@entities/templates/queries";
import { Resource } from "./resource";

import { SourceType } from "@views/templates/state/editro.types";

export const Resources: React.FC = observer(() => {
  const templatesQueries = useTemplatesQueries() as any as TemplateItemQueries;
  const componentsQueries = useComponentQueries() as any as TemplateItemQueries;
  const contextQueries = useContextQueries() as any as TemplateItemQueries;

  return (
    <>
      <Resource
        src={SourceType.Template}
        name="Templates"
        lang="handlebars"
        queries={templatesQueries}
      />
      <Resource
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
      />
    </>
  );
});
