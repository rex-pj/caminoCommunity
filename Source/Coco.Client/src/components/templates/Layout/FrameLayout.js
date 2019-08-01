import React, { Fragment, Component } from "react";
import { Query } from "react-apollo";
import { connect } from "react-redux";
import MasterLayout from "./MasterLayout";
import { Header } from "../../organisms/Containers";
import { GET_LOGGED_USER } from "../../../utils/GraphQLQueries";
import UserContext from "../../../utils/Context/UserContext";
import AuthService from "../../../services/AuthService";
import { getLocalStorageByKey } from "../../../services/StorageService";
import { AUTH_LOGIN_KEY } from "../../../utils/AppSettings";
import PageLoading from "../../molecules/Loading/PageLoading";
import * as avatarActions from "../../../store/actions/avatarActions";

class FrameLayout extends Component {
  constructor(props) {
    super(props);
    this._refetch = null;
    this._isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);
  }

  parseLoggedUser = response => {
    const data = AuthService.parseUserInfo(response);

    const user = {
      lang: "vn",
      authenticationToken: data.tokenkey,
      isLogin: data.isLogin,
      userInfo: {
        ...data
      }
    };
    return user;
  };

  componentWillReceiveProps(nextProps) {
    const { avatarPayload } = nextProps;
    if (!avatarPayload || !this._refetch) {
      return;
    }

    if (avatarPayload.actionType === avatarActions.AVATAR_RELOAD) {
      this._refetch();
    }
  }

  render() {
    const { component: Component } = this.props;
    if (this._isLogin) {
      return (
        <Query query={GET_LOGGED_USER}>
          {({ loading, error, data, refetch }) => {
            if (loading) {
              return <PageLoading {...this.props} />;
            }

            this._refetch = refetch;
            const user = this.parseLoggedUser(data);
            return (
              <UserContext.Provider value={user}>
                <MasterLayout
                  {...this.props}
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
            {...this.props}
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
}

const mapStateToProps = state => {
  return {
    avatarPayload: state.avatarReducer.payload
  };
};

export default connect(mapStateToProps)(FrameLayout);
