import React from "react";
import FrameLayout from "./FrameLayout";
import styled from "styled-components";

const Wrapper = styled.div`
  > .row {
    margin-left: -10px;
    margin-right: -10px;
  }

  > .row > .col {
    padding: 0 10px;
  }
`;

export default ({ component: Component, ...rest }) => {
  return (
    <FrameLayout
      {...rest}
      component={matchProps => (
        <Wrapper className="container px-lg-5">
          <Component {...matchProps} />
        </Wrapper>
      )}
    />
  );
};
