import React, {
  ChangeEvent,
  useCallback,
  useMemo,
  useRef,
  useState,
} from "react";
import cn from "classnames";
import { boxCss, spacingCss } from "@shared/styles";
import { Button, Input } from "antd";
import { editorStore } from "@views/templates/state/editor.store";
import { observer } from "mobx-react-lite";
import { TemplateItem } from "@entities/templates";
import css from "./resources.module.scss";
import { StopOutlined } from "@ant-design/icons";
import { useClickAway } from "ahooks";
import { Icon } from "@views/templates/components/Icon";
import { SourceType } from "@views/templates/state/editro.types";

export const AddNew: React.FC<{
  type: SourceType;
  items: TemplateItem[];
  onAdd: (id: string) => void;
  onCancel: () => void;
}> = observer(({ type, onAdd, items, onCancel }) => {
  const [id, setId] = useState("");
  const ref = useRef<any>();
  useClickAway(() => {
    onCancel();
  }, ref);

  const isUsed = useMemo(() => {
    return (
      editorStore.state.edited.some(
        (x) => x.src === type && x.id.toLowerCase() === id
      ) || items.some((x) => x.id.toLowerCase() === id)
    );
  }, [id, items]);

  const onChange = useCallback(
    (evt: ChangeEvent<HTMLInputElement>) => {
      const value = evt.currentTarget.value;
      setId(value);
    },
    [setId]
  );

  const onAddHandler = useCallback(() => {
    onAdd(id);
  }, [id]);

  return (
    <div
      className={cn(
        boxCss.flex,
        boxCss.alignItemsCenter,
        spacingCss.paddingHorizLg,
        css.item,
        css.itemNew
      )}
      ref={ref}
    >
      <span className={spacingCss.marginRightSm}>
        <Icon type={type} />
      </span>
      <Input
        value={id}
        size="small"
        onChange={onChange}
        bordered={false}
        autoFocus
      />
      <Button
        size="small"
        className={spacingCss.marginLeftSm}
        disabled={isUsed}
        onClick={onAddHandler}
      >
        Add
      </Button>
      <Button
        size="small"
        className={spacingCss.marginLeftSm}
        onClick={onCancel}
      >
        <StopOutlined />
      </Button>
    </div>
  );
});
