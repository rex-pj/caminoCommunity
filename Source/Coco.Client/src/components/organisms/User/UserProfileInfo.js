import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { VerticalList } from "../../atoms/List";
import { format } from "date-fns";

const Root = styled.div`
  position: relative;
  border-radius: ${p => p.theme.borderRadius.normal};
`;

const InfoList = styled(VerticalList)`
  margin-bottom: ${p => p.theme.size.distance};
`;

const ChildItem = styled.li`
  font-size: ${p => p.theme.fontSize.small};
  color: ${p => p.theme.color.dark};
  margin-bottom: ${p => p.theme.size.exSmall};

  span {
    color: inherit;
  }

  svg {
    margin-right: ${p => p.theme.size.exTiny};
  }

  svg,
  path {
    color: ${p => p.theme.color.normal};
  }

  a {
    font-size: ${p => p.theme.fontSize.exSmall};
    font-weight: 600;
  }
`;

function UnserInfoChild(props) {
  const { className, children, icon, isEmail } = props;
  return children ? (
    <ChildItem className={className}>
      {icon ? <FontAwesomeIcon icon={icon} /> : null}
      {!!isEmail ? (
        <a href={`mailto:${children}`}>{children}</a>
      ) : (
        <span>{children}</span>
      )}
    </ChildItem>
  ) : null;
}

export default function(props) {
  const { userInfo } = props;
  return (
    <Root>
      {userInfo ? (
        <InfoList>
          <UnserInfoChild className="text-justify">
            {userInfo.blast}
          </UnserInfoChild>
          <UnserInfoChild icon="map-marked-alt">
            {userInfo.address}
          </UnserInfoChild>
          <UnserInfoChild icon="map-marker-alt">
            {userInfo.country}
          </UnserInfoChild>
          <UnserInfoChild icon="baby">
            {format(userInfo.birthDate, "MMMM, DD YYYY")}
          </UnserInfoChild>
          <UnserInfoChild icon="calendar-alt">
            {format(userInfo.joinedDate, "MMMM, DD YYYY")}
          </UnserInfoChild>
          <UnserInfoChild icon="envelope" isEmail={true}>
            {userInfo.email}
          </UnserInfoChild>
          <UnserInfoChild icon="mobile-alt">{userInfo.mobile}</UnserInfoChild>
        </InfoList>
      ) : null}
    </Root>
  );
}
