import * as React from "react";
import { createRoot, hydrateRoot } from "react-dom/client";
import reportWebVitals from "./reportWebVitals";
import "./index.css";
import App from "./App";
import "core-js/stable";
import "regenerator-runtime/runtime";
import "./assets/css/bootstrap-reboot.min.css";
import "./assets/css/bootstrap-grid.min.css";
import "./assets/css/bootstrap-utilities.min.css";
import "./assets/css/main.css";

const rootElement = document.getElementById("root");
if (rootElement && rootElement.hasChildNodes()) {
  const root = hydrateRoot(rootElement, <></>);
  root.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>
  );
} else if (rootElement) {
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
