import * as React from "react";
import styled from "styled-components";
import ErrorBanner from "../Banner/ErrorBanner";
import bgUrl from "../../../assets/images/notfound-bg.jpg";
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

interface NotFoundProps {
  icon?: IconProp;
  title?: string;
  instruction?: string;
  actionUrl?: string;
  actionText?: string;
}

const NotFound = (props: NotFoundProps) => {
  const { icon, title, instruction, actionUrl, actionText } = props;
  return (
    <Root>
      <ErrorBanner
        title={title}
        icon={icon}
        instruction={instruction}
        actionUrl={actionUrl}
        actionText={actionText}
      ></ErrorBanner>
    </Root>
  );
};

export default NotFound;
