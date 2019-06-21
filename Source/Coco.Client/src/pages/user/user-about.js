import React, { Component } from "react";
import { withRouter } from "react-router-dom";
import { Query } from "react-apollo";
import { GET_USER_INFO_TO_UPDATE } from "../../utils/GraphQLQueries";
import About from "../../components/organisms/User/About";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";

export default withRouter(
  class extends Component {
    render() {
      const { userId } = this.props;
      return (
        <Query
          query={GET_USER_INFO_TO_UPDATE}
          variables={{
            criterias: {
              userId
            }
          }}
        >
          {({ loading, error, data }) => {
            if (loading) {
              return <Loading>Loading</Loading>;
            }
            if (error) {
              return <ErrorBlock>Error</ErrorBlock>;
            }
            return <About userInfo={data} />;
          }}
        </Query>
      );
    }
  }
);
