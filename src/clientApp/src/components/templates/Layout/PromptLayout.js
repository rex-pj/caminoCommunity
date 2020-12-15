import React from "react";
import styled from "styled-components";
import MasterLayout from "./MasterLayout";

const Root = styled.div`
  background-color: ${(p) => p.theme.color.lighter};
  height: 100%;
`;

const Container = styled.div`
  position: relative;
  height: 100%;
  width: 100%;
  padding-top: 50px;
`;

const Wrap = styled.div`
  width: 750px;
  max-width: 100%;
  background-color: ${(p) => p.theme.color.primaryLight};
  min-height: 500px;
  border-radius: ${(p) => p.theme.borderRadius.medium};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  margin: auto;
  overflow: hidden;

  .row {
    height: 100%;
  }
`;

export default function ({ component: Component, ...rest }) {
  return (
    <MasterLayout
      {...rest}
      component={(matchProps) => (
        <Root>
          <Container>
            <Wrap>
              <Component {...matchProps} />
            </Wrap>
          </Container>
        </Root>
      )}
    />
  );
}
