import React from "react";
// import ReactDOM from "react-dom";
import { hydrate, render } from "react-dom";
import App from "./App";
// import { render } from "react-snapshot";

it("renders without crashing", () => {
  const div = document.createElement("div");
  if (div.hasChildNodes()) {
    hydrate(<App />, rootElement);
  } else {
    render(<App />, rootElement);
  }
  // render(<App />, div);
  ReactDOM.unmountComponentAtNode(div);
});
