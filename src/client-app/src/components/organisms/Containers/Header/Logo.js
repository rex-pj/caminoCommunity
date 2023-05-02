import React from "react";
import styled from "styled-components";
import { AnchorLink } from "../../../atoms/Links";
import { PageInfo } from "../../../../utils/Constants";
import { Image } from "../../../atoms/Images";
import logoUrl from "../../../../assets/images/logo.png";

const LogoImageLink = styled.a`
  float: left;
  height: calc(${(p) => p.theme.size.normal} - 2px);
  margin-top: 2px;
  margin-bottom: 2px;

  ${Image} {
    max-height: 100%;
    vertical-align: middle;
  }
`;

const TitleLink = styled(AnchorLink)`
  float: left;
  height: ${(p) => p.theme.size.normal};
  :hover {
    text-decoration: none;
  }
`;

const LogoTitle = styled.h2`
  display: inline-block;
  color: ${(p) => p.theme.color.neutralTitle};
  font-family: "Chonburi", cursive;
  font-size: 1.6rem;
  margin: 1px 0;
  height: ${(p) => p.theme.size.normal};
  line-height: ${(p) => p.theme.size.normal};
  background: ${(p) => p.theme.gradientColor.light};
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  font-weight: 600;
`;

const TitleHead = styled.span`
  margin-right: 3px;
  color: inherit;
  vertical-align: middle;
`;

const TitleTail = styled.span`
  color: inherit;
  vertical-align: middle;
`;

const Logo = () => {
  return (
    <div className="clearfix">
      <LogoImageLink
        href="/"
        className="me-0 me-md-1"
        title={`${PageInfo.Slogan}`}
      >
        <Image
          src={logoUrl}
          alt={`/${PageInfo.BrandName} ${PageInfo.BrandDescription}`}
        />
      </LogoImageLink>
      <TitleLink
        to="/"
        className="d-none d-md-block"
        title={`${PageInfo.Slogan}`}
      >
        <LogoTitle>
          <TitleHead>{PageInfo.BrandName}</TitleHead>
          <TitleTail>{PageInfo.BrandDescription}</TitleTail>
        </LogoTitle>
      </TitleLink>
    </div>
  );
};

export default Logo;
