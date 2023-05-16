import * as React from "react";
import BodyLayout from "./BodyLayout";
import styled from "styled-components";
import { Header } from "../../organisms/Containers";
import Notifications from "../../organisms/Notification/Notifications";
import Modal from "../../organisms/Modals/Modal";

const Wrapper = styled.div`
  > .row {
    margin-left: -12px;
    margin-right: -12px;
  }

  > .row > .col {
    padding: 0 12px;
  }
`;

interface Props {
  isLoading: boolean;
  hasData: boolean;
  hasError: boolean;
  children?: any;
}

// The layout for User profile page
const ProfileLayout = (props: Props) => {
  const { children, isLoading, hasData, hasError } = props;
  return (
    <>
      <Header />
      <BodyLayout isLoading={isLoading} hasData={hasData} hasError={hasError}>
        <Wrapper className="container container-md container-sm container-lg px-lg-5 px-md-2 px-sm-1">
          {children}
        </Wrapper>
      </BodyLayout>
      <Notifications />
      <Modal />
    </>
  );
};

export default ProfileLayout;
