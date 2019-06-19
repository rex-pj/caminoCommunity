import React, { Fragment } from "react";
import { Query } from "react-apollo";
import MasterLayout from "./MasterLayout";
import { Header } from "../../organisms/Containers";
import { GET_LOGGED_USER } from "../../../utils/GraphQLQueries";
import UserContext from "../../../utils/Context/UserContext";
import AuthService from "../../../services/AuthService";
import { getLocalStorageByKey } from "../../../services/StorageService";
import { AUTH_LOGIN_KEY } from "../../../utils/AppSettings";

export default function({ component: Component, ...rest }) {
  function parseLoggedUser(response) {
    const data = AuthService.parseUserInfo(response);

    const user = {
      lang: "vn",
      authenticatorToken: data.tokenkey,
      isLogin: data.isLogin,
      userInfo: {
        displayName: data.displayName,
        userHashedId: data.userHashedId
      }
    };
    return user;
  }

  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);

  if (isLogin) {
    return (
      <Query query={GET_LOGGED_USER}>
        {({ loading, error, data }) => {
          if (loading) {
            return <div>Loading</div>;
          }

          const user = parseLoggedUser(data);
          return (
            <UserContext.Provider value={user}>
              <MasterLayout
                {...rest}
                component={matchProps => (
                  <Fragment>
                    <Header />
                    <Component {...matchProps} />
                  </Fragment>
                )}
              />
            </UserContext.Provider>
          );
        }}
      </Query>
    );
  } else {
    return (
      <UserContext.Provider value={{}}>
        <MasterLayout
          {...rest}
          component={matchProps => (
            <Fragment>
              <Header />
              <Component {...matchProps} />
            </Fragment>
          )}
        />
      </UserContext.Provider>
    );
  }
}
