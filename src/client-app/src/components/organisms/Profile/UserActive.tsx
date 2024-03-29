import * as React from "react";
import styled from "styled-components";
import ConfirmationBanner from "../Banner/ConfirmationBanner";
import { PromptLayout } from "../../templates/Layout";
import bgUrl from "../../../assets/images/activeuser-bg.jpg";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

const Root = styled.div`
  background: url(${bgUrl}) no-repeat center;
  width: 100%;
  background-size: cover;
  position: relative;
  z-index: 1;
  height: 100%;
  min-height: 500px;
`;

interface Props {
  icon?: IconProp;
  title: string;
  instruction?: string;
  actionUrl?: string;
  actionText?: string;
}

const UserActive = (props: Props) => {
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
