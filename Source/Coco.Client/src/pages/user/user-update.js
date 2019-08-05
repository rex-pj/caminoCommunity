import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { Query, Mutation } from "react-apollo";
import ProfileUpdateFrom from "../../components/organisms/User/ProfileUpdateForm";
import { connect } from "react-redux";
import { raiseError } from "../../store/commands";
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

    this.state = { isFormEnabled: true };
  }
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
                  onUpdate={(e, updateUserProfile) =>
                    this.onUpdate(e, updateUserProfile, canEdit)
                  }
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
    showValidationError: (title, message) => {
      raiseError(dispatch, title, message, "#");
    }
  };
};

export default connect(
  null,
  mapDispatchToProps
)(withRouter(UserUpdate));
