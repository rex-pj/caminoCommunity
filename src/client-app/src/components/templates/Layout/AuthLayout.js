import React from "react";
import MasterLayout from "./MasterLayout";
import AuthBanner from "../../../components/organisms/Banner/AuthBanner";
import { isTokenValid } from "../../../services/authService";
import styled from "styled-components";

const Root = styled.div`
  background-color: ${(p) => p.theme.color.neutralBg};
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
  background-color: ${(p) => p.theme.color.primaryBg};
  min-height: 500px;
  border-radius: ${(p) => p.theme.borderRadius.medium};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  margin: auto;
  overflow: hidden;

  .row {
    height: 100%;
  }
`;

// The layout for login, signup or forgot password
export default ({ component: Component, ...rest }) => {
  return (
    <MasterLayout
      {...rest}
      component={(matchProps) => (
        <Root>
          <Container>
            <Wrap>
              {isTokenValid() ? (
                <AuthBanner
                  icon="exclamation-triangle"
                  title="You are logged in"
                  instruction="Please back to the homepage to follow other farms"
                  actionUrl="/"
                  actionText="Go to homepage"
                />
              ) : (
                <Component {...matchProps} />
              )}
            </Wrap>
          </Container>
        </Root>
      )}
    />
  );
};
