import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import * as actionTypes from "../../store/actions";
import client from "../../utils/GraphQL/GraphQLClient";
import SignUpForm from "../../components/organisms/Auth/SignUpForm";
import { ADD_USER } from "../../utils/GraphQL/GraphQLQueries";
import { Errors } from "../../utils/LangData/Notifies";
import UserContext from "../../utils/Context/UserContext";

class SignUpPage extends Component {
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

  signUp = async data => {
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
        this.props.notifyError(error, this.context);
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
    notifyError: (error, userContext) => {
      if (
        error &&
        error.networkError &&
        error.networkError.result &&
        error.networkError.result.errors
      ) {
        const errors = error.networkError.result.errors;

        errors.forEach(item => {
          dispatch({
            type: actionTypes.NOTIFICATION,
            payload: {
              type: "error",
              title: "Đăng ký KHÔNG thành công",
              description:
                Errors &&
                Errors[item.extensions.code] &&
                Errors[item.extensions.code][userContext.lang]
                  ? Errors[item.extensions.code][userContext.lang]
                  : "Có lỗi xảy ra trong quá trình đăng ký, thấn tải lại trang và thử lại",
              url: "/auth/signup"
            }
          });
        });
      } else {
        dispatch({
          type: actionTypes.NOTIFICATION,
          payload: {
            type: "error",
            title: "Đăng ký KHÔNG thành công",
            description:
              "Có lỗi xảy ra trong quá trình đăng ký, hãy nhấn tải lại trang hoặc quay lại sau",
            url: "/auth/signup"
          }
        });
      }
    },
    showValidationError: (title, message) =>
      dispatch({
        type: actionTypes.NOTIFICATION,
        payload: {
          type: "error",
          title: title,
          description: message,
          url: "#"
        }
      })
  };
};

SignUpPage.contextType = UserContext;

export default connect(
  null,
  mapDispatchToProps
)(withRouter(SignUpPage));
