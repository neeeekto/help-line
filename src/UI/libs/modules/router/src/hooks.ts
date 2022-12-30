import { useLocation } from 'react-router';

export const usePathMaker = () => {
  const { pathname } = useLocation();
  return (...segments: string[]) =>
    [pathname, ...(segments || [])]
      .filter(Boolean)
      .join('/')
      .replace(/\/\//g, '/');
};
