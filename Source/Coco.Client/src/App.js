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
import UserContext from "./utils/Context/UserContext";
import LoggedUser from "./utils/Context/LoggedUser";
import { ApolloProvider } from "react-apollo";
import { authClient } from "./utils/GraphQLClient";

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
    // await AuthService.getLoggedUserInfo()
    //   .then(user => {
    //     this.setState({
    //       lang: "vn",
    //       authenticatorToken: user.tokenkey,
    //       isLogin: user.isLogin,
    //       userInfo: {
    //         displayName: user.displayName,
    //         userHashedId: user.userHashedId
    //       }
    //     });
    //   })
    //   .catch(error => {
    //     // console.log(error);
    //   });
  };

  async componentDidMount() {
    await this.initializeSiteContext();
  }

  login = async () => {
    // await AuthService.getLoggedUserInfo()
    //   .then(user => {
    //     this.setState({
    //       authenticatorToken: user.tokenkey,
    //       isLogin: user.isLogin,
    //       userInfo: {
    //         displayName: user.displayName,
    //         userHashedId: user.userHashedId
    //       }
    //     });
    //   })
    //   .catch(error => {
    //     // console.log(error);
    //   });
  };

  logout = () => {
    if (this.state.isLogin) {
      this.setState({
        authenticatorToken: null,
        isLogin: false,
        userInfo: {}
      });
    }
  };

  render() {
    return (
      <ApolloProvider client={authClient}>
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
                <ProfileLayout
                  exact={true}
                  path={[
                    "/profile/:userId",
                    "/profile/:userId/:pageName",
                    "/profile/:userId/:pageName/page/:pageNumber"
                  ]}
                  component={() => <AsyncPage page="./pages/user/profile" />}
                />
                <DefaultLayout
                  exact={true}
                  path={[
                    "/",
                    "/page/:pageNumber",
                    "/feeds",
                    "/feeds/page/:pageNumber"
                  ]}
                  component={() => <AsyncPage page="./pages/feeds" />}
                />
                <PromptLayout
                  path="*"
                  component={() => <AsyncPage page="./pages/error/not-found" />}
                />
              </Switch>
            </BrowserRouter>
          </Provider>
        </UserContext.Provider>
      </ApolloProvider>
    );
  }
}

export default App;
