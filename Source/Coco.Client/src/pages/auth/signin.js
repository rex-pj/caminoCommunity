import React, { Component } from "react";
import loadable from "@loadable/component";
import { defaultClient } from "../../utils/GraphQLClient";
import { SIGNIN } from "../../utils/GraphQLQueries";
import { getError } from "../../utils/Helper";
import UserContext from "../../utils/Context/UserContext";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { raiseError } from "../../store/notify";
import AuthService from "../../services/AuthService";
const SignInForm = loadable(() =>
  import("../../components/organisms/Auth/SignInForm")
);

class SingnInPage extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;

    this.state = {
      isFormEnabled: true
    };
  }

  // #region Life Cycle
  componentDidMount() {
    this._isMounted = true;
  }

  componentWillUnmount() {
    this._isMounted = false;
  }
  // #endregion Life Cycle

  showValidationError = (title, message) => {
    this.props.showValidationError(title, message);
  };

  signIn = async data => {
    if (this._isMounted) {
      this.setState({ isFormEnabled: false });
    }

    await defaultClient
      .query({
        query: SIGNIN,
        variables: {
          signinModel: data
        }
      })
      .then(result => {
        const { data } = result;
        const { signin } = data;

        if (!signin || !signin.isSuccess) {
          this.props.notifyError(data.signin.errors, this.context.lang);
          if (this._isMounted) {
            this.setState({ isFormEnabled: true });
          }
          return;
        }

        AuthService.setLogin(
          data.signin.userInfo,
          data.signin.authenticatorToken
        );
        this.context.login();
        this.props.history.push("/");
      })
      .catch(error => {
        if (this._isMounted) {
          this.setState({ isFormEnabled: true });
        }
        this.props.notifyError(error, this.context.lang);
      });
  };

  render() {
    return (
      <SignInForm
        onSignin={this.signIn}
        showValidationError={this.showValidationError}
        isFormEnabled={this.state.isFormEnabled}
      />
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    notifyError: (error, lang) => {
      if (
        error &&
        error.networkError &&
        error.networkError.result &&
        error.networkError.result.errors
      ) {
        const errors = error.networkError.result.errors;

        errors.forEach(item => {
          raiseError(
            dispatch,
            "Đăng nhập KHÔNG thành công",
            getError(item.extensions.code, lang),
            "/auth/signin"
          );
        });
      } else {
        raiseError(
          dispatch,
          "Đăng nhập KHÔNG thành công",
          getError("ErrorOccurredTryRefeshInputAgain", lang),
          "/auth/signin"
        );
      }
    },

    showValidationError: (title, message) => {
      raiseError(dispatch, title, message, "#");
    }
  };
};

SingnInPage.contextType = UserContext;

export default connect(
  null,
  mapDispatchToProps
)(withRouter(SingnInPage));
