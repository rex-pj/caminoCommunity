import React from "react";

let userData = {
  lang: "vn",
  authenticatorToken: null
};

const UserContext = React.createContext(userData);

export default UserContext;
