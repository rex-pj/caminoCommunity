import React, { Component } from "react";
import ForgotPasswordForm from "../../components/organisms/Auth/ForgotPasswordForm";
import { Mutation } from "react-apollo";
import { defaultClient } from "../../utils/GraphQLClient";
import { FORGOT_PASSWORD } from "../../utils/GraphQLQueries";
import { getError } from "../../utils/Helper";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { raiseError } from "../../store/commands";
import SessionContext from "../../utils/Context/SessionContext";

class SingnInPage extends Component {
  constructor(props) {
    super(props);
    this._isMounted = false;

    this.state = {
      isFormEnabled: true,
      shouldRedirect: false
    };
  }

  // #region Life Cycle
  componentDidMount() {
    this._isMounted = true;
  }

  componentWillUnmount() {
    this._isMounted = false;
    clearTimeout();
  }
  // #endregion Life Cycle

  showValidationError = (title, message) => {
    this.props.showValidationError(title, message);
  };

  onForgotPassword = async (data, forgotPassword) => {
    if (this._isMounted) {
      this.setState({ isFormEnabled: false });
    }

    if (forgotPassword) {
      await forgotPassword({
        variables: {
          criterias: data
        }
      })
        .then(response => {
          const { data } = response;
          const { forgotPassword } = data;

          if (!forgotPassword || !forgotPassword.isSuccess) {
            this.props.notifyError(
              data.forgotPassword.errors,
              this.context.lang
            );
            if (this._isMounted) {
              this.setState({ isFormEnabled: true });
            }
            return;
          }

          this.props.history.push("/");
        })
        .catch(error => {
          if (this._isMounted) {
            this.setState({ isFormEnabled: true });
          }
          this.props.notifyError(error, this.context.lang);
        });
    }
  };

  render() {
    return (
      <Mutation mutation={FORGOT_PASSWORD} client={defaultClient}>
        {forgotPassword => (
          <ForgotPasswordForm
            onForgotPassword={data =>
              this.onForgotPassword(data, forgotPassword)
            }
            showValidationError={this.showValidationError}
            isFormEnabled={this.state.isFormEnabled}
          />
        )}
      </Mutation>
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

SingnInPage.contextType = SessionContext;

export default connect(
  null,
  mapDispatchToProps
)(withRouter(SingnInPage));
