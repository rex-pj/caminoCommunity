import React from "react";
import styled from "styled-components";
import ConfirmationBanner from "../Banner/ConfirmationBanner";
import { PromptLayout } from "../../templates/Layout";

const Root = styled.div`
  background: url(${`${process.env.PUBLIC_URL}/images/activeuser-bg.jpg`})
    no-repeat center;
  width: 100%;
  background-size: cover;
  position: relative;
  z-index: 1;
  height: 100%;
  min-height: 500px;
`;

const UserActive = (props) => {
  const { icon, title, instruction, actionUrl, actionText } = props;
  return (
    <PromptLayout>
      <Root>
        <ConfirmationBanner
          title={title}
          icon={icon}
          instruction={instruction}
          actionUrl={actionUrl}
          actionText={actionText}
        ></ConfirmationBanner>
      </Root>
    </PromptLayout>
  );
};

export default UserActive;
