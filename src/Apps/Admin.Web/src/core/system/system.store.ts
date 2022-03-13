import { observable, action } from "mobx";

export const makeSystemStore = () => {
  const state = observable({});

  return {
    state,
  };
};

export type SystemStore = ReturnType<typeof makeSystemStore>;
