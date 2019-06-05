import React, { Component } from "react";
import { BrowserRouter, Switch } from "react-router-dom";
import { createStore, combineReducers } from "redux";
import { Provider } from "react-redux";
import {
  DefaultLayout,
  DetailLayout,
  FarmPageLayout,
  ProductPageLayout,
  FrameLayout,
  AuthLayout,
  ProfileLayout,
  PromptLayout
} from "./components/templates/Layout";
import { fas } from "@fortawesome/free-solid-svg-icons";
import { library } from "@fortawesome/fontawesome-svg-core";
import loadable from "@loadable/component";
import notifyReducer from "./store/reducer/notifyReducer";
import summaryNoticeReducer from "./store/reducer/summaryNoticeReducer";
import { getUserInfo } from "./services/AuthService";
import UserContext from "./utils/Context/UserContext";
import LoggedUser from "./utils/Context/LoggedUser";

// Redux
const rootReducer = combineReducers({
  notifyRdc: notifyReducer,
  smrNoticRdc: summaryNoticeReducer
});

const store = createStore(rootReducer);

// Font Awesome
const AsyncPage = loadable(props => import(`${props.page}`));
library.add(fas);

class App extends Component {
  constructor(props) {
    super(props);
    this.state = { ...LoggedUser, login: this.login, logout: this.logout };
  }

  initializeSiteContext = async () => {
    const loggedUser = await getUserInfo();
    this.setState(() => {
      return {
        lang: "vn",
        authenticatorToken: loggedUser.tokenkey,
        isLogin: loggedUser.isLogin,
        userInfo: {
          displayName: loggedUser.displayName,
          userHashedId: loggedUser.userHashedId
        }
      };
    });
  };

  componentDidMount() {
    this.initializeSiteContext();
  }

  login = async () => {
    const loggedUser = await getUserInfo();
    this.setState({
      authenticatorToken: loggedUser.tokenkey,
      isLogin: loggedUser.isLogin,
      userInfo: {
        displayName: loggedUser.displayName,
        userHashedId: loggedUser.userHashedId
      }
    });
  };

  logout = () => {
    if (this.state.isLogin) {
      this.setState(() => {
        return {
          authenticatorToken: null,
          isLogin: false,
          userInfo: {}
        };
      });
    }
  };

  render() {
    return (
      <UserContext.Provider value={this.state}>
        <Provider store={store}>
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
                component={() => (
                  <AsyncPage page="./pages/farm-groups/detail" />
                )}
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
              <PromptLayout
                exact={true}
                path="/auth/signout"
                component={() => <AsyncPage page="./pages/auth/signout" />}
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
                path="/profile?id=:id"
                component={() => <AsyncPage page="./pages/user/profile" />}
              />
              <ProfileLayout
                path="/:id"
                component={() => <AsyncPage page="./pages/user/profile" />}
              />
            </Switch>
          </BrowserRouter>
        </Provider>
      </UserContext.Provider>
    );
  }
}

export default App;
