import { useRouteMatch } from "react-router";

export const usePathMaker = () => {
  const { path } = useRouteMatch();
  return (...segments: string[]) =>
    [path, ...(segments || [])].filter(Boolean).join("/").replaceAll("//", "/");
};
