import React from "react";
import loggedUser from "./LoggedUser";

const UserContext = React.createContext(loggedUser);

export default UserContext;
