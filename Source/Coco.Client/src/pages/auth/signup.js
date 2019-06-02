import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import { raiseError } from "../../store/notify";
import client from "../../utils/GraphQL/GraphQLClient";
import SignUpForm from "../../components/organisms/Auth/SignUpForm";
import { ADD_USER } from "../../utils/GraphQL/GraphQLQueries";
import { getError } from "../../utils/Helper";
import UserContext from "../../utils/Context/UserContext";

class SignUpPage extends Component {
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

  signUp = async data => {
    if (this._isMounted) {
      this.setState({ isFormEnabled: true });
    }

    await client
      .mutate({
        mutation: ADD_USER,
        variables: {
          user: data
        }
      })
      .then(result => {
        this.props.history.push("/auth/signin");
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
      <SignUpForm
        signUp={this.signUp}
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
            "Đăng ký KHÔNG thành công",
            getError(item.extensions.code, lang),
            "/auth/signup"
          );
        });
      } else {
        raiseError(
          dispatch,
          "Đăng ký KHÔNG thành công",
          getError("ErrorOccurredTryRefeshInputAgain", lang),
          "/auth/signup"
        );
      }
    },

    showValidationError: (title, message) => {
      raiseError(dispatch, title, message, "#");
    }
  };
};

SignUpPage.contextType = UserContext;

export default connect(
  null,
  mapDispatchToProps
)(withRouter(SignUpPage));
