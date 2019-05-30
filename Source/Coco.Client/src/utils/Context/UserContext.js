import React from "react";

let userData = {
  lang: "vn",
  authenticatorToken: null,
  userInfo: {},
  isLogin: false
};

const UserContext = React.createContext(userData);

export default UserContext;
