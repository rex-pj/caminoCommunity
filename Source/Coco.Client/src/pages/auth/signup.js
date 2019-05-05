import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { connect } from "react-redux";
import * as actionTypes from "../../store/actions";
import client from "../../utils/GraphQL/GraphQLClient";
import SignUpForm from "../../components/organisms/Auth/SignUpForm";
import { ADD_USER } from "../../utils/GraphQL/GraphQLQueries";

class SignUpPage extends Component {
  showValidationError = (title, message) => {
    this.props.showValidationError(title, message);
  };

  signUp = async data => {
    const body = data;
    await client
      .mutate({
        mutation: ADD_USER,
        variables: {
          user: body
        }
      })
      .then(result => {
        this.props.history.push("/auth/signin");
      })
      .catch(error => {
        this.props.notifyError();
      });
  };

  render() {
    return (
      <SignUpForm
        signUp={this.signUp}
        showValidationError={this.showValidationError}
      />
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    notifyError: () =>
      dispatch({
        type: actionTypes.NOTIFICATION,
        payload: {
          type: "error",
          title: "Đăng ký KHÔNG thành công",
          description: "Hãy nhấn tải lại trang hoặc quay lại sau",
          url: "/auth/signup"
        }
      }),
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

export default connect(
  null,
  mapDispatchToProps
)(withRouter(SignUpPage));
