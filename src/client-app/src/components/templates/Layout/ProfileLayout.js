import React from "react";
import FrameLayout from "./FrameLayout";
import styled from "styled-components";

const Wrapper = styled.div`
  > .row {
    margin-left: -12px;
    margin-right: -12px;
  }

  > .row > .col {
    padding: 0 12px;
  }
`;

// The layout for User profile page
export default ({ component: Component, ...rest }) => {
  return (
    <FrameLayout
      {...rest}
      component={(matchProps) => (
        <Wrapper className="container px-lg-5">
          <Component {...matchProps} />
        </Wrapper>
      )}
    />
  );
};
