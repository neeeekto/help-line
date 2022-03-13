import { setupI18n } from "./i18n/setup";
import { library } from "@fortawesome/fontawesome-svg-core";
import { far } from "@fortawesome/free-regular-svg-icons";
import { fas } from "@fortawesome/free-solid-svg-icons";

export const setupCore = () => {
  setupI18n();
  library.add(far, fas);
};
