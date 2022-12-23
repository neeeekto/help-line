import { MessageLvl } from "@core/system/system.types";

type AntLvl = "info" | "success" | "error" | "warning";
export const messageLvlToAntLvl = (lvl: MessageLvl): AntLvl => {
  switch (lvl) {
    case MessageLvl.Info:
      return "info";
    case MessageLvl.Warning:
      return "warning";
    case MessageLvl.Danger:
      return "error";
    default:
      return "info";
  }
};
