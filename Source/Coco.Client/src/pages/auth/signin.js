import React, { Component } from "react";
import SignInForm from "../../components/organisms/Auth/SignInForm";
import client from "../../utils/GraphQL/GraphQLClient";
import { SIGNIN } from "../../utils/GraphQL/GraphQLQueries";
import { getError } from "../../utils/Helper";
import UserContext from "../../utils/Context/UserContext";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { raiseError } from "../../store/notify";
import { setLogin } from "../../services/AuthService";

class SingnInPage extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;

    this.state = {
      isFormEnabled: false
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
    await client
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
          return;
        }

        setLogin(data.signin.authenticatorToken);
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
