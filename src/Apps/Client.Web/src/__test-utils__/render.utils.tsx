import { act, create, ReactTestRenderer } from "react-test-renderer";
import React from "react";

export const renderAndGet = async (component: React.ReactNode) => {
  let renderer!: ReactTestRenderer;
  await act(() => {
    renderer = create(<>{component}</>);
  });
  return renderer;
};
