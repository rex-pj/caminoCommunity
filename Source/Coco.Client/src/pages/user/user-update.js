import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { Query, Mutation } from "react-apollo";
import ProfileUpdateFrom from "../../components/organisms/User/ProfileUpdateForm";
import { connect } from "react-redux";
import UserContext from "../../utils/Context/UserContext";
import { raiseError, raiseSuccess } from "../../store/commands";
import {
  GET_FULL_USER_INFO,
  UPDATE_USER_PROFILE
} from "../../utils/GraphQLQueries";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { defaultClient } from "../../utils/GraphQLClient";

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
    const { userId } = this.props;
    const { isFormEnabled } = this.state;
    return (
      <Query
        query={GET_FULL_USER_INFO}
        variables={{
          criterias: {
            userId
          }
        }}
        client={defaultClient}
      >
        {({ loading, error, data }) => {
          if (loading) {
            return <Loading>Loading</Loading>;
          }
          if (error) {
            return <ErrorBlock>Error</ErrorBlock>;
          }

          const { fullUserInfo } = data;
          const { result, accessMode } = fullUserInfo;
          const canEdit = accessMode === "CAN_EDIT";

          return (
            <Mutation mutation={UPDATE_USER_PROFILE}>
              {updateUserProfile => (
                <ProfileUpdateFrom
                  onUpdate={e => this.onUpdate(e, updateUserProfile, canEdit)}
                  isFormEnabled={isFormEnabled}
                  userInfo={result}
                  canEdit={canEdit}
                  showValidationError={this.props.showValidationError}
                />
              )}
            </Mutation>
          );
        }}
      </Query>
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
