import { ReactTestInstance, ReactTestRenderer } from "react-test-renderer";

export const makeElementByTypeFilter =
  (type: any) => (element: ReactTestInstance) =>
    element.type === type || // Match non-memo'd
    element.type === type.type; // Match memo'd
