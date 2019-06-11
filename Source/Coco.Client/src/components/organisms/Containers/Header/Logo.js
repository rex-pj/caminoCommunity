import React from "react";
import styled from "styled-components";
import { AnchorLink } from "../../../atoms/Links";
import { PageInfo } from "../../../../utils/Constant";
import { Image } from "../../../atoms/Images";

const LogoImageLink = styled.a`
  float: left;
  height: calc(${p => p.theme.size.normal} - 2px);
  margin-right: 5px;
  margin: 2px 5px 2px 0;

  ${Image} {
    height: 100%;
  }
`;

const TitleLink = styled(AnchorLink)`
  float: left;
  height: ${p => p.theme.size.normal};
  :hover {
    text-decoration: none;
  }
`;

const LogoTitle = styled.h2`
  display: inline-block;
  color: ${p => p.theme.color.light};
  font-family: "Nunito", sans-serif;
  font-size: 24px;
  margin: 1px 0;
  height: ${p => p.theme.size.normal};
`;

const TitleHead = styled.span`
  color: ${p => p.theme.color.light};
  margin-right: 3px;
  height: ${p => p.theme.size.normal};
`;

const TitleTail = styled.span`
  color: ${p => p.theme.color.warning};
  height: ${p => p.theme.size.normal};
`;

export default function () {
  return (
    <div className="clearfix">
      <LogoImageLink href="/">
        <Image
          src={`${process.env.PUBLIC_URL}/images/logo.png`}
          alt={`${PageInfo.BrandName} ${PageInfo.BrandDescription}`}
        />
      </LogoImageLink>
      <TitleLink to="/">
        <LogoTitle>
          <TitleHead>{PageInfo.BrandName}</TitleHead>
          <TitleTail>{PageInfo.BrandDescription}</TitleTail>
        </LogoTitle>
      </TitleLink>
    </div>
  );
};
