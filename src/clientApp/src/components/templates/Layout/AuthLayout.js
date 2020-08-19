import React from "react";
import MasterLayout from "./MasterLayout";
import AuthBanner from "../../../components/organisms/Banner/AuthBanner";
import { getLocalStorageByKey } from "../../../services/StorageService";
import { AUTH_LOGIN_KEY } from "../../../utils/AppSettings";
import styled from "styled-components";

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

function AuthLayout({ component: Component, ...rest }) {
  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);
  return (
    <MasterLayout
      {...rest}
      component={(matchProps) => (
        <Root>
          <Container>
            <Wrap>
              {isLogin ? (
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
}

export default AuthLayout;
