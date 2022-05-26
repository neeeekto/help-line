import React from "react";
import { TicketFilterFeatures } from "@entities/helpdesk/tickets";
import { Tag, Tooltip } from "antd";
import { PresetColorType, PresetStatusColorType } from "antd/lib/_util/colors";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { boxCss, mouseCss, spacingCss } from "@shared/styles";
import { USER_LAZY_ATTENTION_DELAY } from "@shared/constants";
import { LiteralUnion } from "antd/lib/_util/type";
import cn from "classnames";

const FeatureDesc = {
  [TicketFilterFeatures.Automations]: {
    icon: "robot",
    desc: "Can use/using in macros",
    color: "" as LiteralUnion<PresetColorType | PresetStatusColorType, string>,
    text: "",
  },
  [TicketFilterFeatures.Important]: {
    icon: "exclamation-circle",
    desc: "Mark as Important (RED color)",
    color: "" as LiteralUnion<PresetColorType | PresetStatusColorType, string>,
    text: "",
  },
};

export const TicketFilterFeatureTag: React.FC<{ feat: TicketFilterFeatures }> =
  ({ feat }) => {
    const desc = FeatureDesc[feat];
    return (
      <Tooltip
        title={desc.desc}
        placement="topLeft"
        mouseEnterDelay={USER_LAZY_ATTENTION_DELAY}
      >
        <Tag color={desc.color}>
          {desc.icon ? (
            <FontAwesomeIcon size="sm" icon={desc.icon as any} />
          ) : null}
          {desc.text}
        </Tag>
      </Tooltip>
    );
  };
