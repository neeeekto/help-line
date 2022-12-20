import {
  TemplateItemQueries,
  useComponentQueries,
  useContextQueries,
  useTemplatesQueries,
} from "@entities/templates/queries";
import { EditedItem, SourceType } from "@views/templates/state/editro.types";

export const useSaveAll = () => {
  const saveAllTemplates = (
    useTemplatesQueries() as any as TemplateItemQueries
  ).useSaveAllMutation();
  const saveAllComponents = (
    useComponentQueries() as any as TemplateItemQueries
  ).useSaveAllMutation();
  const saveAllContexts = (
    useContextQueries() as any as TemplateItemQueries
  ).useSaveAllMutation();

  return async (changed: EditedItem[]) => {
    const items = changed.map((x) => x.current);
    for (let changedElement of changed) {
      switch (changedElement.src) {
        case SourceType.Component:
          await saveAllComponents.mutate(items);
          return;
        case SourceType.Template:
          await saveAllTemplates.mutate(items);
          return;
        case SourceType.Context:
          await saveAllContexts.mutate(items);
          return;
      }
    }
  };
};
