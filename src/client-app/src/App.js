import React from "react";
import { Router, Switch } from "react-router-dom";
import appRoutes from "./routes/appRoutes";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { library } from "@fortawesome/fontawesome-svg-core";
import loadable from "@loadable/component";
import { ApolloProvider } from "@apollo/client";
import { authClient } from "./graphql/client";
import configureModalStore from "./store/hook-store/modal-store";
import configureAvatarStore from "./store/hook-store/avatar-store";
import configureNotifyStore from "./store/hook-store/notify-store";
import configureContentChangeStore from "./store/hook-store/content-change-store";
import { useQuery } from "@apollo/client";
import { userQueries } from "./graphql/fetching/queries";
import { SessionContext } from "./store/context/session-context";
import {
  parseUserInfo,
  isTokenValid,
  getAuthenticationToken,
} from "./services/authService";
import { createBrowserHistory } from "history";
const AsyncPage = loadable((props) => import(`${props.page}`), {
  cacheKey: (props) => props.page,
});

const history = createBrowserHistory();

configureModalStore();
configureAvatarStore();
configureNotifyStore();
configureContentChangeStore();

// Font Awesome
library.add(fas);

export default () => {
  const { loading, data, refetch, error } = useQuery(
    userQueries.GET_LOGGED_USER,
    {
      client: authClient,
    }
  );

  const relogin = () => {
    return refetch();
  };

  const parseUserResponse = (response) => {
    const currentUser = parseUserInfo(response);
    return {
      lang: "vn",
      authenticationToken: currentUser.tokenkey,
      isLogin: currentUser.isLogin,
      currentUser: currentUser,
    };
  };

  const parseLoggedUser = () => {
    if (error) {
      return { isLogin: false };
    }

    const authToken = getAuthenticationToken();
    if (!authToken) {
      return { isLogin: false };
    }

    // Login success
    const isValid = isTokenValid();
    if (!isValid) {
      return { isLogin: false };
    }

    return parseUserResponse(data);
  };

  return (
    <ApolloProvider client={authClient}>
      <SessionContext.Provider
        value={{ ...parseLoggedUser(), relogin, isLoading: loading }}
      >
        <Router history={history}>
          <Switch>
            {appRoutes.map((route) => {
              var { layout: ComponentLayout, exact, path, page } = route;
              return (
                <ComponentLayout
                  key={route.page}
                  exact={exact}
                  path={path}
                  component={() => <AsyncPage page={`./pages/${page}`} />}
                />
              );
            })}
          </Switch>
        </Router>
      </SessionContext.Provider>
    </ApolloProvider>
  );
};
