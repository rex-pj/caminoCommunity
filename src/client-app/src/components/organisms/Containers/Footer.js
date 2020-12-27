import React from "react";
import styled from "styled-components";

const Root = styled.div`
  background-color: ${(p) => p.theme.color.secondaryBg};
`;

export default () => {
  return (
    <Root>
      <span>Footer</span>
    </Root>
  );
};
