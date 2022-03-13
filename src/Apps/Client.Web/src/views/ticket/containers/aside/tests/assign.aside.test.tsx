import React from "react";
import { render, screen } from "@testing-library/react";
import { TicketAssignAside } from "../assign.aside";

const TTTT = () => <span>learn react</span>;

test("renders learn react link", () => {
  render(<TTTT />);
  const linkElement = screen.getByText(/learn react/i);
  expect(linkElement).toBeInTheDocument();
});
