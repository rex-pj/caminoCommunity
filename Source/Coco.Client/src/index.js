import "@babel/polyfill";
import React from "react";
import { hydrate, render } from "react-dom";
import { loadableReady } from "@loadable/component";
import "./index.css";
import App from "./App";
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
// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
// serviceWorker.register();
