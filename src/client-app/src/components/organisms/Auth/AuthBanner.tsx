import * as React from "react";
import styled from "styled-components";
import { Image } from "../../atoms/Images";
import { PageInfo } from "../../../utils/Constants";
import { SecondaryHeading } from "../../atoms/Heading";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { AnchorLink } from "../../atoms/Links";
import logoUrl from "../../../assets/images/logo.png";
import bgUrl from "../../../assets/images/auth-bg.jpg";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { DefaultTFuncReturn } from "i18next";

const Root = styled.div`
  background: url(${bgUrl}) no-repeat center;
  width: 100%;
  background-size: cover;
  position: relative;
  z-index: 1;
  height: 100%;
  min-height: 500px;
`;

const Instruction = styled.div`
  text-align: center;
  position: absolute;
  top: 50%;
  bottom: 50%;
  left: 0;
  right: 0;
  margin: auto ${(p) => p.theme.size.distance};
  min-height: 180px;
  padding: ${(p) => p.theme.size.distance};
  background: ${(p) => p.theme.rgbaColor.dark};
  border-radius: ${(p) => p.theme.borderRadius.medium};

  ${SecondaryHeading} {
    font-size: ${(p) => p.theme.fontSize.giant};
    color: ${(p) => p.theme.color.whiteText};
    text-transform: uppercase;
  }

  ${Image} {
    display: block;
    margin: auto auto ${(p) => p.theme.size.distance} auto;
    width: ${(p) => p.theme.size.large};
    height: ${(p) => p.theme.size.large};
    background: ${(p) => p.theme.rgbaColor.cyan};
    padding: ${(p) => p.theme.size.tiny};
    border-radius: ${(p) => p.theme.borderRadius.medium};
  }

  svg {
    margin-bottom: ${(p) => p.theme.size.distance};
  }

  svg,
  path {
    font-size: ${(p) => p.theme.fontSize.giant};
    color: ${(p) => p.theme.color.neutralText};
  }

  p {
    color: ${(p) => p.theme.color.neutralText};
    margin-bottom: 0;
    font-size: ${(p) => p.theme.fontSize.small};
    font-weight: 600;
  }

  a {
    color: ${(p) => p.theme.color.neutralText};
    font-size: ${(p) => p.theme.fontSize.small};
    font-weight: 600;
    font-size: ${(p) => p.theme.fontSize.normal};
    margin-top: ${(p) => p.theme.size.exTiny};
    display: block;
    text-decoration: underline;
  }
`;

interface AuthBannerProps {
  imageUrl?: string;
  icon?: IconProp;
  title: string;
  instruction?: string | DefaultTFuncReturn;
  actionUrl?: string;
  actionText?: string;
}

const AuthBanner: React.FC<AuthBannerProps> = (props) => {
  const { imageUrl, icon, title, instruction, actionUrl, actionText } = props;
  return (
    <Root>
      <Instruction>
        {imageUrl ? (
          <Image
            src={logoUrl}
            alt={`${PageInfo.BrandName} ${PageInfo.BrandDescription}`}
          />
        ) : null}
        {icon ? <FontAwesomeIcon icon={icon} /> : null}
        <SecondaryHeading>{title}</SecondaryHeading>
        <p>{instruction}</p>
        {actionUrl && actionText ? (
          <AnchorLink to={actionUrl}>{actionText}</AnchorLink>
        ) : null}
      </Instruction>
    </Root>
  );
};

export default AuthBanner;
