import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { VerticalList } from "../../molecules/List";
import { PrimaryTitle } from "../../atoms/Titles";
import { AnchorLink } from "../../atoms/Links";

const Root = styled.div`
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

const InfoList = styled(VerticalList)`
  margin-bottom: ${(p) => p.theme.size.distance};
`;

const OtherInfo = styled.span`
  color: ${(p) => p.theme.color.neutralText};
`;

const ChildItem = styled.li`
  font-size: ${(p) => p.theme.fontSize.small};
  color: ${(p) => p.theme.color.darkText};
  margin-bottom: ${(p) => p.theme.size.distance};

  span {
    color: inherit;
    text-align: justify;
  }

  ${OtherInfo} {
    color: ${(p) => p.theme.color.secondaryText};
  }

  svg {
    margin-right: ${(p) => p.theme.size.exTiny};
  }

  svg,
  path {
    color: ${(p) => p.theme.color.secondaryText};
  }

  a {
    font-size: ${(p) => p.theme.fontSize.small};
    font-weight: 600;
  }
`;

export default function (props) {
  const { info } = props;
  return (
    <Root>
      <PrimaryTitle>
        <AnchorLink to={info.url}>{info.title}</AnchorLink>
      </PrimaryTitle>
      <InfoList>
        <ChildItem>
          <AnchorLink to={info.url}>
            <FontAwesomeIcon icon="users" />
            <OtherInfo>{info.followingNumber} participants</OtherInfo>
          </AnchorLink>
        </ChildItem>
        <ChildItem>
          <span>{info.description}</span>
        </ChildItem>
      </InfoList>
    </Root>
  );
}
