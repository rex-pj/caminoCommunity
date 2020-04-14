import React from "react";
import { BrowserRouter, Switch } from "react-router-dom";
import {
  DefaultLayout,
  DetailLayout,
  FarmPageLayout,
  ProductPageLayout,
  FrameLayout,
  AuthLayout,
  ProfileLayout,
  PromptLayout,
} from "./components/templates/Layout";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { library } from "@fortawesome/fontawesome-svg-core";
import loadable from "@loadable/component";
import { ApolloProvider } from "@apollo/react-hooks";
import { identityClient } from "./utils/GraphQLClient";
import configureModalStore from "./store/hook-store/modal-store";
import configureAvatarStore from "./store/hook-store/avatar-store";
import configureNotifyStore from "./store/hook-store/notify-store";
import { useQuery } from "@apollo/react-hooks";
import { GET_LOGGED_USER } from "./utils/GraphQlQueries/queries";
import { SessionContext } from "./store/context/SessionContext";
import AuthService from "./services/AuthService";
import { getLocalStorageByKey } from "./services/StorageService";
import { AUTH_LOGIN_KEY } from "./utils/AppSettings";

configureModalStore();
configureAvatarStore();
configureNotifyStore();

// Font Awesome
const AsyncPage = loadable((props) => import(`${props.page}`));
library.add(fas);

export default () => {
  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);
  const { loading, data, refetch, error } = useQuery(GET_LOGGED_USER, {
    client: identityClient,
  });

  const relogin = async () => {
    return refetch();
  };

  const parseLoggedUser = (response) => {
    const userInfo = AuthService.parseUserInfo(response);

    if (error) {
      return {};
    }

    return {
      lang: "vn",
      authenticationToken: userInfo.tokenkey,
      isLogin: userInfo.isLogin,
      user: userInfo,
    };
  };

  const userObj = !!isLogin ? parseLoggedUser(data) : {};

  return (
    <SessionContext.Provider
      value={{ ...userObj, relogin: relogin, isLoading: loading }}
    >
      <ApolloProvider client={identityClient}>
        <BrowserRouter>
          <Switch>
            <DefaultLayout
              exact={true}
              path={["/articles", "/articles/page/:pageNumber"]}
              component={() => <AsyncPage page="./pages/articles" />}
            />
            <DetailLayout
              path="/articles/:id"
              component={() => <AsyncPage page="./pages/articles/detail" />}
            />

            <DefaultLayout
              exact={true}
              path={["/products", "/products/page/:pageNumber"]}
              component={() => <AsyncPage page="./pages/products" />}
            />
            <ProductPageLayout
              exact={true}
              path="/products/:id"
              component={() => <AsyncPage page="./pages/products/detail" />}
            />

            <DefaultLayout
              exact={true}
              path={["/farms", "/farms/page/:pageNumber"]}
              component={() => <AsyncPage page="./pages/farms" />}
            />
            <FarmPageLayout
              exact={true}
              path="/farms/:id"
              component={() => <AsyncPage page="./pages/farms/detail" />}
            />
            <DefaultLayout
              exact={true}
              path={["/farm-groups", "/farm-groups/page/:pageNumber"]}
              component={() => <AsyncPage page="./pages/farm-groups" />}
            />
            <FrameLayout
              exact={true}
              path="/farm-groups/:id"
              component={() => <AsyncPage page="./pages/farm-groups/detail" />}
            />
            <DefaultLayout
              exact={true}
              path={["/news", "/news/page/:pageNumber"]}
              component={() => <AsyncPage page="./pages/news" />}
            />
            <DefaultLayout
              exact={true}
              path="/news/:id"
              component={() => <AsyncPage page="./pages/news/detail" />}
            />
            <AuthLayout
              exact={true}
              path="/auth/forgot-password"
              component={() => (
                <AsyncPage page="./pages/auth/forgot-password" />
              )}
            />
            <AuthLayout
              exact={true}
              path="/auth/signin"
              component={() => <AsyncPage page="./pages/auth/signin" />}
            />
            <AuthLayout
              exact={true}
              path="/auth/signup"
              component={() => <AsyncPage page="./pages/auth/signup" />}
            />
            <PromptLayout
              exact={true}
              path="/auth/signout"
              component={() => <AsyncPage page="./pages/auth/signout" />}
            />
            <ProfileLayout
              exact={true}
              path={[
                "/profile/:userId",
                "/profile/:userId/:pageName",
                "/profile/:userId/:pageName/page/:pageNumber",
              ]}
              component={() => <AsyncPage page="./pages/user/profile" />}
            />
            <DefaultLayout
              exact={true}
              path={[
                "/",
                "/page/:pageNumber",
                "/feeds",
                "/feeds/page/:pageNumber",
              ]}
              component={() => <AsyncPage page="./pages/feeds" />}
            />
            <PromptLayout
              exact={true}
              path="/user/active/:email/:key"
              component={() => <AsyncPage page="./pages/user/active" />}
            />
            <PromptLayout
              exact={true}
              path="/user/reset-password/:email/:key"
              component={() => <AsyncPage page="./pages/user/reset-password" />}
            />
            <PromptLayout
              path="/error"
              component={() => <AsyncPage page="./pages/error/index" />}
            />
            <PromptLayout
              path="*"
              component={() => <AsyncPage page="./pages/error/not-found" />}
            />
          </Switch>
        </BrowserRouter>
      </ApolloProvider>
    </SessionContext.Provider>
  );
};
