import React from "react";
import styled from "styled-components";

const Root = styled.div`
  background-color: ${p => p.theme.color.secondary};
`;

export default function () {
  return (
    <Root>
      <span>Footer</span>
    </Root>
  );
};
