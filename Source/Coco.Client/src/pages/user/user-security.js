import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { Mutation } from "react-apollo";
import UpdatePasswordForm from "../../components/organisms/User/UpdatePasswordForm";
import { connect } from "react-redux";
import UserContext from "../../utils/Context/UserContext";
import { raiseError, raiseSuccess } from "../../store/commands";
import { UPDATE_USER_PASSWORD } from "../../utils/GraphQLQueries";

class UserUpdate extends Component {
  constructor(props) {
    super(props);

    this._isMounted = false;
    this.state = { isFormEnabled: true };
  }

  // #region Life Cycle
  componentDidMount() {
    this._isMounted = true;
  }

  componentWillUnmount() {
    this._isMounted = false;
  }
  // #endregion Life Cycle

  onUpdate = async (data, updateUserProfile, canEdit) => {
    if (!canEdit) {
      return;
    }

    if (this._isMounted) {
      this.setState({ isFormEnabled: true });
    }

    if (updateUserProfile) {
      await updateUserProfile({
        variables: {
          user: data
        }
      })
        .then(result => {
          if (this._isMounted) {
            this.setState({ isFormEnabled: true });
          }
          this.props.notifySuccess(this.context.user.lang);
        })
        .catch(error => {
          if (this._isMounted) {
            this.setState({ isFormEnabled: true });
          }
          this.props.notifyError(error, this.context.user.lang);
        });
    }
  };

  render() {
    const { canEdit } = this.props;
    const { isFormEnabled } = this.state;
    return (
      <Mutation mutation={UPDATE_USER_PASSWORD}>
        {updateUserPassword => (
          <UpdatePasswordForm
            onUpdate={e => this.onUpdate(e, updateUserPassword, canEdit)}
            isFormEnabled={isFormEnabled}
            canEdit={canEdit}
            showValidationError={this.props.showValidationError}
          />
        )}
      </Mutation>
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    notifyError: (error, lang) => {
      if (error) {
        raiseError(
          dispatch,
          "Có lỗi xảy ra trong quá trình cập nhật",
          "Kiểm tra lại thông tin và thử lại",
          "/"
        );
      }
    },
    notifySuccess: lang => {
      raiseSuccess(dispatch, "Thay đổi thành công", null, "/");
    },
    showValidationError: (title, message) => {
      raiseError(dispatch, title, message, "#");
    }
  };
};

UserUpdate.contextType = UserContext;

export default connect(
  null,
  mapDispatchToProps
)(withRouter(UserUpdate));
