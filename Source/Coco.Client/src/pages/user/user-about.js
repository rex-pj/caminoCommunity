import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { Query } from "react-apollo";
import { GET_FULL_USER_INFO } from "../../utils/GraphQLQueries";
import About from "../../components/organisms/User/About";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { defaultClient } from "../../utils/GraphQLClient";

export default withRouter(
  class extends Component {
    render() {
      const { userId } = this.props;
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

            return <About userInfo={result} canEdit={canEdit} />;
          }}
        </Query>
      );
    }
  }
);
