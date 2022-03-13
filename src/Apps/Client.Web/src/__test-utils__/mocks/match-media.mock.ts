import React from "react";

global.matchMedia =
  global.matchMedia ||
  function () {
    return {
      matches: false,
      onchange: null,
      addListener: jest.fn(), // deprecated
      removeListener: jest.fn(), // deprecated
      addEventListener: jest.fn(),
      removeEventListener: jest.fn(),
      dispatchEvent: jest.fn(),
    };
  };
