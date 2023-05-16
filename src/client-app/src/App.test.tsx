import * as React from "react";
import * as ReactDOM from "react-dom";
import { hydrate, render } from "react-dom";
import App from "./App";

it("renders without crashing", () => {
  const div = document.createElement("div");
  if (div.hasChildNodes()) {
    hydrate(<App />, div);
  } else {
    render(<App />, div);
  }
  ReactDOM.unmountComponentAtNode(div);
});
