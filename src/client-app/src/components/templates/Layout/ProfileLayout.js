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
export default ({ children }) => {
  return (
    <FrameLayout>
      <Wrapper className="container container-md container-sm container-lg px-lg-5 px-md-2 px-sm-1">
        {children}
      </Wrapper>
    </FrameLayout>
  );
};
