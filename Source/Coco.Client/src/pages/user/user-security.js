import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { Mutation } from "react-apollo";
import UpdatePasswordForm from "../../components/organisms/User/UpdatePasswordForm";
import { connect } from "react-redux";
import UserContext from "../../utils/Context/UserContext";
import { raiseError, raiseSuccess, openModal } from "../../store/commands";
import { UPDATE_USER_PASSWORD } from "../../utils/GraphQLQueries";
import AuthService from "../../services/AuthService";

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

  onUpdateConfirmation = () => {
    this.props.openConfirmRedirectModal({
      title: "Bạn sẽ cần phải thoát và đăng nhập lại",
      executeButtonName: "Đồng ý",
      modalType: "confirm-redirect",
      executeUrl: "/auth/signout"
    });
  };

  onUpdatePassword = async (data, updateUserProfile, canEdit) => {
    if (!canEdit) {
      return;
    }

    if (this._isMounted) {
      this.setState({ isFormEnabled: true });
    }

    if (updateUserProfile) {
      await updateUserProfile({
        variables: {
          criterias: data
        }
      })
        .then(response => {
          if (this._isMounted) {
            this.setState({ isFormEnabled: true });
          }

          const { data } = response;
          const { updatePassword } = data;
          const { result } = updatePassword;

          AuthService.setLogin(null, result.authenticationToken);
          this.props.notifySuccess(this.context.user.lang);
          this.onUpdateConfirmation();
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
            onUpdate={e =>
              this.onUpdatePassword(e, updateUserPassword, canEdit)
            }
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
    notifySuccess: () => {
      raiseSuccess(
        dispatch,
        "Thay đổi thành công, vui lòng đăng nhập lại",
        null,
        "/"
      );
    },
    showValidationError: (title, message) => {
      raiseError(dispatch, title, message, "#");
    },
    openConfirmRedirectModal: data => {
      openModal(dispatch, data);
    }
  };
};

UserUpdate.contextType = UserContext;

export default connect(
  null,
  mapDispatchToProps
)(withRouter(UserUpdate));
