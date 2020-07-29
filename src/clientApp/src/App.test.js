import React from "react";
import ReactDOM, { hydrate, render } from "react-dom";
import App from "./App";
// import { render } from "react-snapshot";

it("renders without crashing", () => {
  const div = document.createElement("div");
  if (div.hasChildNodes()) {
    hydrate(<App />, div);
  } else {
    render(<App />, div);
  }
  // render(<App />, div);
  ReactDOM.unmountComponentAtNode(div);
});
