import React, { Suspense, useMemo } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { library } from "@fortawesome/fontawesome-svg-core";
import { ApolloProvider, useQuery } from "@apollo/client";
import { authClient } from "./graphql/client";
import configureModalStore from "./store/hook-store/modal-store";
import configureAvatarStore from "./store/hook-store/avatar-store";
import configureNotifyStore from "./store/hook-store/notify-store";
import configureContentChangeStore from "./store/hook-store/content-change-store";
import { userQueries } from "./graphql/fetching/queries";
import { SessionContext } from "./store/context/session-context";
import { parseUserSession, isTokenValid } from "./services/AuthLogic";
import { ThemeProvider } from "styled-components";
import * as theme from "./utils/Theme";
import { LoadingBar } from "./components/molecules/NotificationBars";
import "./i18n";
import "../../../../type-checkings/file-import";
const FeedsPage = React.lazy(() => import("./pages/feeds"));
const ArticlesPage = React.lazy(() => import("./pages/articles"));
const ArticleDetailPage = React.lazy(() => import("./pages/articles/detail"));
const ArticleUpdatePage = React.lazy(() => import("./pages/articles/update"));
const ProductsPage = React.lazy(() => import("./pages/products"));
const ProductDetailPage = React.lazy(() => import("./pages/products/detail"));
const ProductUpdatePage = React.lazy(() => import("./pages/products/update"));
const FarmsPage = React.lazy(() => import("./pages/farms"));
const FarmDetailPage = React.lazy(() => import("./pages/farms/detail"));
const FarmUpdatePage = React.lazy(() => import("./pages/farms/update"));
const ShoppingCartPage = React.lazy(() => import("./pages/shopping-cart"));
const CommunitiesPage = React.lazy(() => import("./pages/communities"));
const CommunityDetailPage = React.lazy(
  () => import("./pages/communities/detail")
);
const ForgotPasswordPage = React.lazy(
  () => import("./pages/auth/forgot-password")
);
const LoginPage = React.lazy(() => import("./pages/auth/login"));
const SignupPage = React.lazy(() => import("./pages/auth/signup"));
const LogoutPage = React.lazy(() => import("./pages/auth/logout"));
const ProfilePage = React.lazy(() => import("./pages/user/profile"));
const SearchPage = React.lazy(() => import("./pages/feeds/search"));
const UserActivePage = React.lazy(() => import("./pages/user/user-active"));
const ResetPasswordPage = React.lazy(
  () => import("./pages/user/reset-password")
);
const ErrorPage = React.lazy(() => import("./pages/error/index"));
const NotFoundPage = React.lazy(() => import("./pages/error/not-found"));

configureModalStore();
configureAvatarStore();
configureNotifyStore();
configureContentChangeStore();

// Font Awesome
library.add(fas);

const App: React.FC = () => {
  const { loading, data, refetch, error } = useQuery(
    userQueries.GET_LOGGED_USER,
    {
      client: authClient,
    }
  );

  const relogin = () => {
    return refetch();
  };

  const parseLoggedUser = useMemo(
    () => () => {
      if (error) {
        return { isLogin: false };
      }

      function parseUserResponse(response) {
        // Login success
        const isValid = isTokenValid();
        if (!isValid) {
          return { isLogin: false };
        }

        const currentUser = parseUserSession(response);
        return currentUser;
      }

      return parseUserResponse(data);
    },
    [data, error]
  );

  const sessionContext = () => {
    return { ...parseLoggedUser(), relogin, isLoading: loading };
  };

  return (
    <ApolloProvider client={authClient}>
      <SessionContext.Provider value={sessionContext()}>
        <ThemeProvider theme={theme}>
          <Suspense fallback={<LoadingBar />}>
            <BrowserRouter>
              <Routes>
                {[
                  "/",
                  "/page/:pageNumber",
                  "/feeds",
                  "/feeds/page/:pageNumber",
                ].map((path) => {
                  return (
                    <Route
                      key={path}
                      path={path}
                      element={<FeedsPage />}
                    ></Route>
                  );
                })}
                {["/articles", "/articles/page/:pageNumber"].map((path) => {
                  return (
                    <Route
                      key={path}
                      path={path}
                      element={<ArticlesPage />}
                    ></Route>
                  );
                })}
                <Route
                  path="/articles/:id"
                  element={<ArticleDetailPage />}
                ></Route>
                <Route
                  path="/articles/update/:id"
                  element={<ArticleUpdatePage />}
                ></Route>
                {["/products", "/products/page/:pageNumber"].map((path) => {
                  return (
                    <Route
                      key={path}
                      path={path}
                      element={<ProductsPage />}
                    ></Route>
                  );
                })}
                <Route
                  path="/products/:id"
                  element={<ProductDetailPage />}
                ></Route>
                <Route
                  path="/products/update/:id"
                  element={<ProductUpdatePage />}
                ></Route>
                <Route
                  path="/shopping-cart"
                  element={<ShoppingCartPage />}
                ></Route>
                {["/farms", "/farms/page/:pageNumber"].map((path) => {
                  return (
                    <Route
                      key={path}
                      path={path}
                      element={<FarmsPage />}
                    ></Route>
                  );
                })}
                <Route path="/farms/:id" element={<FarmDetailPage />}></Route>
                <Route
                  path="/farms/update/:id"
                  element={<FarmUpdatePage />}
                ></Route>
                <Route
                  path="/communities"
                  element={<CommunitiesPage />}
                ></Route>
                <Route
                  path="/communities/page/:pageNumber"
                  element={<CommunitiesPage />}
                ></Route>
                <Route
                  path="/communities/:id"
                  element={<CommunityDetailPage />}
                ></Route>
                <Route
                  path="/auth/forgot-password"
                  element={<ForgotPasswordPage />}
                ></Route>
                <Route path="/auth/login" element={<LoginPage />}></Route>
                <Route path="/auth/signup" element={<SignupPage />}></Route>
                <Route path="/auth/logout" element={<LogoutPage />}></Route>
                <Route
                  path="/profile/:userId/*"
                  element={<ProfilePage />}
                ></Route>
                <Route path="/search" element={<SearchPage />}></Route>
                <Route path="/search/:keyword" element={<SearchPage />}></Route>
                {[
                  "/user/active/:email/:key",
                  "/user/active/:email/:key+",
                  "/user/active/:email/*",
                ].map((path) => {
                  return (
                    <Route
                      key={path}
                      path={path}
                      element={<UserActivePage />}
                    ></Route>
                  );
                })}
                {[
                  "/user/reset-password/:email/:key",
                  "/user/reset-password/:email/:key+",
                  "/user/reset-password/:email/*",
                ].map((path) => {
                  return (
                    <Route
                      key={path}
                      path={path}
                      element={<ResetPasswordPage />}
                    ></Route>
                  );
                })}
                <Route path="/error" element={<ErrorPage />}></Route>
                <Route path="*" element={<NotFoundPage />}></Route>
              </Routes>
            </BrowserRouter>
          </Suspense>
        </ThemeProvider>
      </SessionContext.Provider>
    </ApolloProvider>
  );
};

export default App;
