import "@babel/polyfill";
import React from "react";
import { hydrate, render } from "react-dom";
import { loadableReady } from "@loadable/component";
// import { render } from "react-snapshot";
import "./index.css";
import App from "./App";
// import * as serviceWorker from "./serviceWorker";
require("dotenv").config();

// Allow the passed state to be garbage-collected
delete window.__APOLLO_STORE__;

const rootElement = document.getElementById("root");
if (rootElement.hasChildNodes()) {
  loadableReady(() => {
    hydrate(<App />, rootElement);
  });
} else {
  render(<App />, rootElement);
}

// render(<App />, document.getElementById("root"));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
// serviceWorker.register();
