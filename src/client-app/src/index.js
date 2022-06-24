import React from "react";
import { createRoot, hydrateRoot } from "react-dom/client";
import reportWebVitals from "./reportWebVitals";
import "./index.css";
import App from "./App";
import "core-js/stable";
import "regenerator-runtime/runtime";

// Allow the passed state to be garbage-collected
delete window.__APOLLO_STORE__;

const rootElement = document.getElementById("root");
if (rootElement.hasChildNodes()) {
  const root = hydrateRoot(rootElement);
  root.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>
  );
} else {
  const root = createRoot(rootElement);
  root.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>
  );
}

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
