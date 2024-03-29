import * as React from "react";
import styled from "styled-components";

const Root = styled.div`
  background-color: ${(p) => p.theme.color.primaryBg};
`;

const Footer = () => {
  return (
    <Root>
      <span>Footer</span>
    </Root>
  );
};

export default Footer;
