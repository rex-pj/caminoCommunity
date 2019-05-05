import React, { Component } from "react";
import { BrowserRouter, Switch } from "react-router-dom";
import {
  DefaultLayout,
  DetailLayout,
  FarmPageLayout,
  ProductPageLayout,
  FrameLayout,
  AuthLayout,
  ProfileLayout
} from "./components/templates/Layout";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { library } from "@fortawesome/fontawesome-svg-core";
import loadable from "@loadable/component";

// Font Awesome
const AsyncPage = loadable(props => import(`${props.page}`));
library.add(fas);

class App extends Component {
  render() {
    return (
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
            path="/auth/signin"
            component={() => <AsyncPage page="./pages/auth/signin" />}
          />
          <AuthLayout
            exact={true}
            path="/auth/signup"
            component={() => <AsyncPage page="./pages/auth/signup" />}
          />

          <DefaultLayout
            exact={true}
            path={["/feeds", "/feeds/page/:pageNumber"]}
            component={() => <AsyncPage page="./pages/feeds" />}
          />
          <DefaultLayout
            exact={true}
            path={["/", "/page/:pageNumber"]}
            component={() => <AsyncPage page="./pages/feeds" />}
          />
          <ProfileLayout
            path="/:id"
            component={() => <AsyncPage page="./pages/user/profile" />}
          />
        </Switch>
      </BrowserRouter>
    );
  }
}

export default App;
